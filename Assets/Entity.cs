using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public HexCell Location;

    public List<Trait> Traits = new List<Trait>();
    public Faction Faction { get; set; }

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
    private readonly Dictionary<Type, Trait> TraitCache = new Dictionary<Type, Trait>();

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

    public int ActionPoints { get; set; }

}