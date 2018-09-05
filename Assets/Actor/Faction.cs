using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour
{
    public Color Color;

    public List<Actor> Members = new List<Actor>();

    public Sprite Sprite;

    public Actor Leader { get; set; }

    public int Gold { get; set; }

    public int Food { get; set; }

    public int Manpower { get; set; }

    public List<HexCell> KnownHexes = new List<HexCell>();

    public List<HexCell> VisibleHexes = new List<HexCell>();

    public override string ToString()
    {
        return name;
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
            hex.Known = true;
            hex.Visble = true;
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

    public void SetLeader(Actor leader)
    {
        Leader = leader;
        Leader.Faction = this;
        Leader.AddTrait(new HexClaimer(Leader));
    }

    public void AddMember(Actor person)
    {
        Members.Add(person);
        person.Faction = this;
    }

    public void TakeTurn()
    {
        Leader.TakeTurn();
        foreach (var member in Members)
        {
            member.TakeTurn();
        }
    }

    public void ResetFog()
    {
        foreach (var hex in VisibleHexes)
        {
            hex.Visble = false;
        }

        VisibleHexes.Clear();
    }
}