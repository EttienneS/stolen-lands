using System.Collections.Generic;

public class Player : Sentient
{
    public override List<ActorAction> GetActions()
    {
        return new List<ActorAction>();
    }

    public override void TakeAction(List<ActorAction> allActions)
    {
        ControlPanelController.Instance.ClearPlayerActions();

        foreach (var action in allActions)
        {
            ControlPanelController.Instance.AddAction(action);
        }
    }

   
}