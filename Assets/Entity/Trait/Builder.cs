using System.Collections.Generic;
using System.Linq;

public class Builder : Trait
{
    public Builder() { }

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
        return StructureController.Instance.AvailableBuildings(Owner).Cast<object>().ToList();
    }

    private static int Build(Entity entity, object target)
    {
        return StructureController.Instance.Build(entity, target as Structure).Cost;
    }

    public override void Start()
    {
    }

    public override void Finish()
    {
    }

    public override string Save()
    {
        return "0";
    }

    public override void Load(string data)
    {
    }
}