using System.Collections.Generic;
using System.Linq;

public class Builder : Trait
{
    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>
        {
            new ActorAction("Build", Owner, GetBuildings, BuildingCost, Build)
        };

        return actions;
    }

    private int BuildingCost(Actor actor, object target)
    {
        return BuildingController.Instance.GetBuilding(target.ToString()).Cost;
    }

    private List<object> GetBuildings(Actor actor)
    {
        return BuildingController.Instance.AvailableBuildings(actor.ActionPoints).Cast<object>().ToList();
    }

    private int Build(Actor actor, object target)
    {
        return BuildingController.Instance.Build(actor, target.ToString()).Cost; ;
    }

    public override void DoPassive()
    {

    }
}