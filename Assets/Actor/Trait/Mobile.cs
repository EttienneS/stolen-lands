using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mobile : Trait
{
    public Mobile(Actor owner) : base(owner)
    {
    }



    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();

        if (Owner == ActorController.Instance.Player)
        {
            actions.Add(new ActorAction("Move", Owner, 1, DiscoverReachableCells, MoveToCell));
        }

        return actions;
    }

    public static List<HexCell> DiscoverReachableCells(Actor actor)
    {
        return HexGrid.Instance.GetReachableCells(actor.Location, actor.ActionsAvailable);
    }

    public static void MoveToCell(Actor actor, List<HexCell> target)
    {
        var dest = target[0];

        var path = HexGrid.Instance.FindPath(actor.Location, dest);

        actor.ActionsAvailable -= HexGrid.Instance.GetPathCost(path);
        actor.Move(dest);
    }

}