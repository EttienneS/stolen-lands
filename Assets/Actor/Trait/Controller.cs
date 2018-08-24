using System.Collections.Generic;

public class Controller : Trait
{
    public Controller(Actor owner) : base(owner)
    {
        UnderControl = new List<Controlled>();
    }

    public List<Controlled> UnderControl { get; set; }

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