using System.Collections.Generic;
using System.Linq;

public class Builder : Trait
{
    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();

        if (Owner.Location != null)
        {
            if (!Owner.Location.Entities.OfType<Structure>().Any() && Owner.ActionPoints > 0)
            {
                foreach (var sturcture in GetBuildableStructures())
                {
                    actions.Add(new ActorAction("Build", Owner, Build, sturcture));
                }
            }
        }

        return actions;
    }

    private List<object> GetBuildableStructures()
    {
        return StructureController.Instance.AvailableBuildings(Owner.ActionPoints).Cast<object>().ToList();
    }

    private int Build(Entity entity, object target)
    {
        return StructureController.Instance.Build(entity, target.ToString()).Cost;
    }

    public override void Start()
    {
    }

    public override void Finish()
    {
    }
}