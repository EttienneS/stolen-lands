public class ActorAction
{
    public delegate void Act(Actor actor, object target);

    public delegate object Discover(Actor actor);

    public Actor ActionContext;

    public int Cost;

    public ActorAction(Actor actionContext, int cost, Discover discover, Act act)
    {
        ActionContext = actionContext;
        DiscoverAction = discover;
        ActAction = act;
        Cost = cost;
    }

    public Discover DiscoverAction { get; set; }
    public Act ActAction { get; set; }

    public void Execute()
    {
        ActAction(ActionContext, DiscoverAction(ActionContext));
    }
}