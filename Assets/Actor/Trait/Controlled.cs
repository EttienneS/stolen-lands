using System.Collections.Generic;

public class Controlled : Trait
{
    public Controller Controller { get; set; }

    public Controlled(Actor owner, Controller controller) : base(owner)
    {
        Controller = controller;
    }

    public override List<ActorAction> GetActions()
    {
        // any spesific context actions go here
        // controlled has its actions taken away by controller
        return new List<ActorAction>();
    }

    public override string ToString()
    {
        return Owner +  " controlled by: " + Controller.Owner.Name;
    }
}

public class Controller : Trait
{
    public List<Controlled> UnderControl { get; set; }

    public Controller(Actor owner) : base(owner)
    {
        UnderControl = new List<Controlled>();
    }

    public static void AddControl(Actor controllingActor, Actor controlledActor)
    {
        var controller = controllingActor.AddTrait(new Controller(controllingActor));
        controller.UnderControl.Add(controlledActor.AddTrait(new Controlled(controlledActor, controller)));
    }

    public override List<ActorAction> GetActions()
    {
        var allActions = new List<ActorAction>();

        // is directly controlled give all actions to controller
        foreach (var controlled in UnderControl)
        {
            foreach (var trait in controlled.Owner.Traits)
            {
                allActions.AddRange(trait.GetActions());
            }
        }

        return allActions;
    }

    public override string ToString()
    {
        return "Controlls: " + UnderControl.Count + " other entities";
    }
}
