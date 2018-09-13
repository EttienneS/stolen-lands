public class ActorAction
{
    public delegate int Act(Entity entity, object target);

    public string ActionName;

    public Entity EntityContext;

    public object Target;

    public ActorAction(string name, Entity entityContext, Act act, object target)
    {
        EntityContext = entityContext;
        ActAction = act;
        ActionName = name;

        Target = target;
    }

    private Act ActAction { get; }

    public void Invoke()
    {
        EntityContext.ActionPoints -= ActAction(EntityContext, Target);
    }

    public override string ToString()
    {
        return ActionName + " >> " + Target;
    }
}