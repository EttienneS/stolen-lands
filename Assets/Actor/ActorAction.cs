public class ActorAction
{
    public delegate void Act(Actor actor, object target);

    public delegate object Discover(Actor actor);

    public ActorAction(Discover discover, Act act)
    {
        DiscoverAction = discover;
        ActAction = act;
    }

    public Discover DiscoverAction { get; set; }
    public Act ActAction { get; set; }

    public void Execute(Actor actor)
    {
        ActAction(actor, DiscoverAction(actor));
    }
}