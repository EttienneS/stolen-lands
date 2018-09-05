using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private readonly Dictionary<Type, Trait> TraitCache = new Dictionary<Type, Trait>();

    private MeshRenderer _meshRenderer;
    public HexCell Location;

    public List<Trait> Traits = new List<Trait>();

    public Sprite Sprite { get; set; }

    private MeshRenderer MeshRenderer
    {
        get
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }

            return _meshRenderer;
        }
    }

    

    public void Start()
    {
        var res = 16;

        var texture = TextureCreator.GetTexture(null, res, TextureHelper.GetRandomColor());
        Sprite = Sprite.Create(texture, new Rect(new Vector2(), new Vector2(res, res)), new Vector2());

        MeshRenderer.material.mainTexture = texture;
    }

    void OnMouseDown()
    {
        SystemController.Instance.SetSelectedActor(this);
    }

    public void EnableOutline()
    {
        var outline = Shader.Find("Custom/Outline");

        MeshRenderer.material.shader = outline;
        MeshRenderer.material.SetFloat("_Outline", 0.04f);
        MeshRenderer.material.SetColor("_OutlineColor", Color.red);
    }

    public void DisableOtline()
    {
        MeshRenderer.material.SetFloat("_Outline", 0f);
    }


    public int ActionsAvailable { get; set; }

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

    public void StartTurn()
    {
        ActionsAvailable = 5;
    }

    public void TakeTurn()
    {
        var player = GetTrait<Player>();
        if (player == null)
        {
            var sentient = GetTrait<Sentient>();
            if (sentient != null)
            {
                sentient.TakeAction(AvailableActions);
            }
        }
        else
        {
            if (SystemController.Instance.SelectedActor == this)
            {
                player.TakeAction(AvailableActions);
            }
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