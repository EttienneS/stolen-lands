using System.Collections.Generic;

public class Building : Trait
{
    public override List<ActorAction> GetActions()
    {
        return new List<ActorAction>();
    }

    public override void DoPassive()
    {
    }
}