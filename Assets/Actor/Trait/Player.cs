using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Sentient
{
    public int SpentActions { get; set; }

    public override int ActionsAvailable
    {
        get { return 10 - SpentActions; }
    }

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
            if (action.Cost <= ActionsAvailable)
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