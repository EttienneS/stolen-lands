using System.Collections.Generic;

public abstract class Trait
{
    public Actor Owner { get; set; }

    public string Name { get; set; }

    public abstract List<ActorAction> GetActions();

    public abstract void DoPassive();

}