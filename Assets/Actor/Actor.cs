using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Actor : MonoBehaviour
{
    private readonly Dictionary<Type, Trait> TraitCache = new Dictionary<Type, Trait>();

    public List<ActorAction> ActorActions = new List<ActorAction>();
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
        var allActions = ActorActions;

        foreach (var trait in Traits)
        {
            if (trait.Actions != null)
            {
                allActions.AddRange(trait.Actions);
            }
        }

        if (allActions.Any())
        {
            allActions[Random.Range(0, allActions.Count - 1)].Execute(this);
        }
    }

    public T GetTrait<T>() where T : Trait
    {
        var type = typeof(T);
        if (TraitCache.ContainsKey(type))
        {
            // only ever have one trait of a type
            return (T) TraitCache[type];
        }

        var trait = Traits.OfType<T>().FirstOrDefault();
        if (trait != null)
        {
            TraitCache.Add(type, trait);
            return trait;
        }

        return null;
    }
}