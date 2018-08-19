using System.Collections.Generic;

public class Trait
{
    public Actor Owner { get; set; }

    public List<ActorAction> Actions { get; set; }

    public string Name { get; set; }

    public Trait(Actor owner)
    {
        Owner = owner;
    }
}