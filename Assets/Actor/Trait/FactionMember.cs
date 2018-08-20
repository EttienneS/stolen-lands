
using System.Collections.Generic;

public class FactionMember : Trait
{
    public FactionMember(Actor owner, Faction faction) : base(owner)
    {
        Faction = faction;
    }

    public Faction Faction { get; set; }

    public override string ToString()
    {
        return "Member of : " + Owner.name;
    }

    public override List<ActorAction> GetActions()
    {
        // ask faction for acitons
        return new List<ActorAction>();
    }


}