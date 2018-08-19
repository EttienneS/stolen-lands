using System.Collections.Generic;

public class Trait
{
    public Trait(Actor owner)
    {
        // just a linkback to the original trait owner (access parent traits in child and vice versa)
        Owner = owner;
    }

    public Actor Owner { get; set; }

    public List<ActorAction> Actions { get; set; }

    public string Name { get; set; }
}