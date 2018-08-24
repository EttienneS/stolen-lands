using System.Collections.Generic;

public class Controlled : Trait
{
    public Controlled(Actor owner, Controller controller) : base(owner)
    {
        Controller = controller;
    }

    public Controller Controller { get; set; }

    public override List<ActorAction> GetActions()
    {
        // any spesific context actions go here
        // controlled has its actions taken away by controller
        return new List<ActorAction>();
    }

    public override string ToString()
    {
        return Owner + " controlled by: " + Controller.Owner.name;
    }
}