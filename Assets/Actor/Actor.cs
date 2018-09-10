using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private readonly Dictionary<Type, Trait> TraitCache = new Dictionary<Type, Trait>();

    private readonly List<SpriteRenderer> _indicators = new List<SpriteRenderer>();

    private MeshRenderer _meshRenderer;

    public SpriteRenderer ActionIndicatorPrefab;
    public HexCell Location;

    public List<Trait> Traits = new List<Trait>();

    public Faction Faction { get; set; }

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

    public void Update()
    {
        if (Faction != ActorController.Instance.PlayerFaction)
            return;

        var radius = 0.5f;
        if (_indicators.Count != ActionPoints)
        {
            foreach (var i in _indicators)
            {
                Destroy(i.gameObject);
            }

            _indicators.Clear();

            for (var i = 0; i < ActionPoints; i++)
            {
                var indicator = Instantiate(ActionIndicatorPrefab, transform);

                var angle = i * Mathf.PI * 2f / ActionPoints;

                indicator.transform.localPosition = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius,
                    indicator.transform.localPosition.z);

                _indicators.Add(indicator);
            }
        }
    }

    public void Start()
    {
        var res = 32;

        var texture = TextureCreator.GetTexture(null, res, TextureHelper.GetRandomColor());
        Sprite = Sprite.Create(texture, new Rect(new Vector2(), new Vector2(res, res)), new Vector2());

        MeshRenderer.material.mainTexture = texture;
    }

    private void OnMouseDown()
    {
        SystemController.Instance.SetSelectedActor(this);
    }

    public void EnableOutline()
    {
        var outline = Shader.Find("Custom/Outline");

        MeshRenderer.material.shader = outline;
        MeshRenderer.material.SetFloat("_Outline", 0.05f);
        MeshRenderer.material.SetColor("_OutlineColor", Faction.Color);
    }

    public void DisableOutline()
    {
        MeshRenderer.material.SetFloat("_Outline", 0f);
    }

    public void StartTurn()
    {
        ActionPoints = 3;
        foreach (var trait in Traits)
        {
            trait.DoPassive();
        }
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
}