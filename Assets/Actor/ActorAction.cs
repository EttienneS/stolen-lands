using System.Collections.Generic;

public class ActorAction
{
    public delegate int Act(Entity entity, object target);

    public delegate object Discover(Entity entity);

    public delegate int GetActionCost(Entity entity, object target);

    public string ActionName;

    public Entity EntityContext;

    public ActorAction(string name, Entity entityContext, Discover discover, GetActionCost cost, Act act)
    {
        EntityContext = entityContext;
        DiscoverAction = discover;
        ActAction = act;
        GetCost = cost;
        ActionName = name;
    }

    public Discover DiscoverAction { get; set; }
    public Act ActAction { get; set; }
    public GetActionCost GetCost { get; set; }

    public bool CanExecute(Actor actor, HexCell context)
    {
        return GetCost(actor, context) <= actor.ActionPoints;
    }
}