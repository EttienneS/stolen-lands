using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private readonly Dictionary<Type, Trait> TraitCache = new Dictionary<Type, Trait>();
    public HexCell Location;

    public Mind Mind;

    public List<Trait> Traits = new List<Trait>();
    public Faction Faction { get; set; }

    public int ActionPoints { get; set; }

    public List<ActorAction> AvailableActions
    {
        get
        {
            var allActions = new List<ActorAction>();
            foreach (var trait in Traits)
            {
                allActions.AddRange(trait.GetActions());
            }

            return allActions;
        }
    }

    public T GetTrait<T>() where T : Trait
    {
        var type = typeof(T);
        if (TraitCache.ContainsKey(type))
        {
            // should only ever have one trait of a type so return the first value
            return (T)TraitCache[type];
        }

        var trait = Traits.OfType<T>().FirstOrDefault();
        if (trait != null)
        {
            TraitCache.Add(type, trait);
            return trait;
        }

        return null;
    }

    public T AddTrait<T>(T trait) where T : Trait
    {
        var newTrait = GetTrait<T>();
        if (newTrait == null)
        {
            Traits.Add(trait);
            trait.Owner = this;
        }

        newTrait = GetTrait<T>();

        return newTrait;
    }

    public abstract void TakeTurn();
    public abstract void StartTurn();
    public abstract void EndTurn();
}