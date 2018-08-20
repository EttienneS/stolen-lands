using UnityEngine;
using System.Collections.Generic;

public class Faction : Trait
{
    public Faction(Actor owner, Actor leader) : base(owner)
    {
        Members = new List<FactionMember>();
        Controller.AddControl(leader, owner);
        Leader = leader;
    }

    public Actor Leader { get; set; }

    public List<FactionMember> Members { get; set; }
   
    public static Faction GetFaction(Transform parent, Actor leader)
    {
        var name = ActorHelper.GetRandomName();
        var gameObject = new GameObject(name);

        gameObject.AddComponent(typeof(Actor));
        gameObject.transform.parent = parent;

        var actor = gameObject.GetComponent<Actor>();

        actor.Location = leader.Location;
        actor.AddTrait(new Faction(actor, leader));
        actor.AddTrait(new HexClaimer(actor));
        actor.AddTrait(new Controlled(actor, null));
        actor.Instantiate(name, TextureHelper.GetRandomColor());

        return actor.GetTrait<Faction>();
    }

    public override List<ActorAction> GetActions()
    {
        return new List<ActorAction>();
    }

    public override string ToString()
    {
        return "Faction : " + Owner.Name;
    }
}
