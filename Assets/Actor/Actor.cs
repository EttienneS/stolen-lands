using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Actor : MonoBehaviour
{
    private Dictionary<Type, Trait> TraitCache = new Dictionary<Type, Trait>();

    public Color Color;

    public HexCell Location;

    public string Name;

    public Sprite Sprite;

    public List<Trait> Traits = new List<Trait>();

    public void Instantiate(string name, Color color)
    {
        Name = name;
        Color = color;

        // resolution of sprite
        var res = 16;
        Sprite = Sprite.Create(TextureCreator.GetTexture(null, res, color),
            new Rect(new Vector2(), new Vector2(res, res)), new Vector2());
    }

    public void TakeTurn()
    {
        if (GetTrait<Controlled>() != null)
        {
            // controlled actors turns are taken by their controllers
            return;
        }

        var allActions = new List<ActorAction>();

        foreach (var trait in Traits)
        {
            allActions.AddRange(trait.GetActions());
        }

        if (allActions.Any())
        {
            allActions[Random.Range(0, allActions.Count - 1)].Execute();
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
        }

        newTrait = GetTrait<T>();

        return newTrait;
    }
}