using System.Collections.Generic;

public abstract class Trait
{
    public Trait(Actor owner)
    {
        // just a linkback to the original trait owner (access parent traits in child and vice versa)
        Owner = owner;
    }

    public Actor Owner { get; set; }

    public string Name { get; set; }

    public abstract List<ActorAction> GetActions();
}