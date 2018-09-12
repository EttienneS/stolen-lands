using System.Collections.Generic;
using System.Linq;

public class Builder : Trait
{
    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>
        {
            new ActorAction("Build", Owner, GetBuildableStructures, GetStructureCost, Build)
        };

        return actions;
    }

    private int GetStructureCost(Entity entity, object target)
    {
        return StructureController.Instance.GetBuilding(target.ToString()).Cost;
    }

    private List<object> GetBuildableStructures(Entity entity)
    {
        if (entity.Location.Entities.OfType<Structure>().Any())
        {
            return new List<object>();
        }

        return StructureController.Instance.AvailableBuildings(entity.ActionPoints).Cast<object>().ToList();
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