using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Faction : MonoBehaviour
{
    public GameObject _border;
    public Color Color;

    public List<HexCell> Demense = new List<HexCell>();
    public int Gold = 0;
    public List<Structure> Holdings = new List<Structure>();

    public List<HexCell> KnownHexes = new List<HexCell>();

    public List<Actor> Members = new List<Actor>();

    public Sprite Sprite;

    public List<HexCell> VisibleHexes = new List<HexCell>();

    public void AddHolding(Structure building)
    {
        Holdings.Add(building);

        var renderer = building.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            renderer.material.color = Color;
        }

        building.Faction = this;
        building.transform.SetParent(transform);
    }

    public void AddMember(Actor person)
    {
        Members.Add(person);
        person.Faction = this;

        person.transform.SetParent(transform);
    }

    public void Claim(HexCell hex)
    {
        if (!Demense.Contains(hex) && hex.Owner == null)
        {
            hex.Owner = this;
            Demense.Add(hex);
        }

        RefreshBorder();
    }
    public void EndTurn()
    {
        foreach (var member in Members)
        {
            member.TakeTurn();
            member.EndTurn();
        }

        foreach (var holding in Holdings)
        {
            holding.EndTurn();
        }
    }

    public void Instantiate(string name, Color color)
    {
        this.name = name;
        Color = color;

        // resolution of sprite
        var res = 16;
        Sprite = Sprite.Create(TextureCreator.GetTexture(null, res, color),
            new Rect(new Vector2(), new Vector2(res, res)), new Vector2());
    }

    public void LearnHex(HexCell hex)
    {
        if (!KnownHexes.Contains(hex))
        {
            KnownHexes.Add(hex);
        }

        if (!VisibleHexes.Contains(hex))
        {
            VisibleHexes.Add(hex);
        }

        if (ActorController.Instance.PlayerFaction == this)
        {
            hex.MoveToLayer(GameHelpers.VisibleLayer);
        }
    }

    public void RefreshBorder()
    {
        if (_border != null)
        {
            Destroy(_border);
        }

        _border = GameHelpers.DrawBorder(Members[0].Location, Demense, Color);
    }

    public void ResetFog()
    {
        if (ActorController.Instance.PlayerFaction == this)
        {
            foreach (var hex in VisibleHexes)
            {
                hex.MoveToLayer(GameHelpers.KnownLayer);

                foreach (var item in hex.Entities.OfType<Actor>())
                {
                    item.gameObject.MoveToUnknownLayer();
                }
            }
        }

        VisibleHexes.Clear();
    }

    public void StartTurn()
    {
        foreach (var member in Members)
        {
            member.StartTurn();
        }

        foreach (var holding in Holdings)
        {
            holding.StartTurn();
        }
    }

    public override string ToString()
    {
        return name;
    }
}