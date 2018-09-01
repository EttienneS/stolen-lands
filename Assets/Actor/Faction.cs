using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour
{
    public Color Color;

    public List<FactionMember> Members = new List<FactionMember>();

    public Sprite Sprite;

    public Actor Leader { get; set; }

    public int Gold { get; set; }

    public int Food { get; set; }

    public int Manpower { get; set; }

    public override string ToString()
    {
        return name;
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
        Leader.AddTrait(new FactionMember(Leader, this));
        Leader.AddTrait(new HexClaimer(Leader));
    }

    public void AddMember(Actor person)
    {
        Members.Add(person.AddTrait(new FactionMember(person, this)));
    }
}