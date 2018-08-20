public class ActorAction
{
    public delegate void Act(Actor actor, object target);

    public delegate object Discover(Actor actor);

    public Actor ActionContext;

    public ActorAction(Actor actionContext, Discover discover, Act act)
    {
        ActionContext = actionContext;
        DiscoverAction = discover;
        ActAction = act;
    }

    public Discover DiscoverAction { get; set; }
    public Act ActAction { get; set; }

    public void Execute()
    {
        ActAction(ActionContext, DiscoverAction(ActionContext));
    }
}