using System.Collections.Generic;

public class ActorAction
{
    public delegate int Act(Actor actor, HexCell target);

    public delegate List<HexCell> Discover(Actor actor);

    public delegate int GetActionCost(Actor actor, HexCell target);

    public string ActionName;

    public Actor ActorContext;


    public ActorAction(string name, Actor actorContext, Discover discover, GetActionCost cost, Act act)
    {
        ActorContext = actorContext;
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
        return GetCost(actor, context) <= actor.ActionsAvailable;
    }
}