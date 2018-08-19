using System.Collections.Generic;

public class Trait
{
    public Trait(Actor owner)
    {
        Owner = owner;
    }

    public Actor Owner { get; set; }

    public List<ActorAction> Actions { get; set; }

    public string Name { get; set; }
}