using System.Collections.Generic;

public class Player : Sentient
{
    public Player(Actor owner) : base(owner)
    {
    }

    public override List<ActorAction> GetActions()
    {
        return new List<ActorAction>();
    }

    public override void TakeAction(List<ActorAction> allActions)
    {
        HexGrid.Instance.ClearPlayerActions();

        foreach (var action in allActions)
        {
            foreach (var actionCell in action.DiscoverAction(action.ActorContext))
            {
                if (action.CanExecute(Owner, actionCell))
                {
                    HexGrid.Instance.AddPlayerActionToCanvas(this, actionCell, action);
                }
            }
        }
    }


    public void ActionExecuted(ActorAction executedAction, int cost)
    {
        Owner.ActionsAvailable -= cost;
        Owner.TakeTurn();
    }
}