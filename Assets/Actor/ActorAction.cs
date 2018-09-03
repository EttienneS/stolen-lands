using System.Collections.Generic;

public class ActorAction
{
    public delegate void Act(Actor actor, List<HexCell> target);

    public delegate List<HexCell> Discover(Actor actor);

    public string ActionName;

    public Actor ActorContext;

    public int Cost;

    public ActorAction(string name, Actor actorContext, int cost, Discover discover, Act act)
    {
        ActorContext = actorContext;
        DiscoverAction = discover;
        ActAction = act;
        Cost = cost;
        ActionName = name;
    }

    public Discover DiscoverAction { get; set; }
    public Act ActAction { get; set; }

    public void Execute()
    {
        ActAction(ActorContext, DiscoverAction(ActorContext));
    }
}