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

    private int BuildingCost(Actor actor, HexCell target)
    {
        return 1;
    }

    private List<object> GetBuildings(Actor actor)
    {
        var buildings = new List<object>();

        //if (!actor.Location.Buildings.Any())
        //{
        //}

        return buildings;
    }

    private int Build(Actor actor, HexCell target)
    {

        return 1;
    }

    public override void DoPassive()
    {

    }
}