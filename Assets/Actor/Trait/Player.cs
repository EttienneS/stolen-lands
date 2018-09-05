using System.Collections.Generic;

public class Player : Sentient
{
    public Player(Actor owner) : base(owner)
    {
    }

    public int SpentActions { get; set; }
    
    public override List<ActorAction> GetActions()
    {
        return new List<ActorAction>();
    }

    public override void TakeAction(List<ActorAction> allActions)
    {
        HexGrid.Instance.ClearPlayerActions();

        foreach (var action in allActions)
        {
            if (action.Cost <= Owner.ActionsAvailable)
            {
                foreach (var actionCell in action.DiscoverAction(action.ActorContext))
                {
                    HexGrid.Instance.AddPlayerActionToCanvas(this, actionCell, action);
                }
            }
        }
    }


    public void RefreshActions(ActorAction executedAction)
    {
        SpentActions += executedAction.Cost;
        Owner.TakeTurn();
    }
}