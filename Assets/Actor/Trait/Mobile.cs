using System.Collections.Generic;
using UnityEngine;

public class Mobile : Trait
{
    public Mobile(Actor owner) : base(owner)
    {
    }

    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();

        if (Owner.Faction == ActorController.Instance.PlayerFaction)
        {
            actions.Add(new ActorAction("Move", Owner, DiscoverReachableCells, CostToCell, MoveToCell));
        }
        else
        {
            // AI MOVEMENT HERE
            // actions.Add(new ActorAction("Move", Owner, 0, DiscoverReachableCells, MoveToCell));
        }

        return actions;
    }

    public int CostToCell(Actor actor, HexCell cell)
    {
        return Pathfinder.GetPathCost(Pathfinder.FindPath(actor.Location, cell)) - Owner.Location.TravelCost;
    }

    public override void DoPassive()
    {
    }

    public int MoveToCell(HexCell target)
    {
        if (Owner.Location != null)
        {
            var path = Pathfinder.FindPath(Owner.Location, target);
            var cost = CostToCell(Owner, target);

            // move along path
            foreach (var cell in path)
            {
                Move(cell);
            }

            return cost;
        }

        // if owner is nowhere, move instantly
        Move(target);

        return 0;
    }

    private void Move(HexCell target)
    {
        // moves instantly to location
        // use MoveToCell to move along a path
        Owner.Location = target;
        Owner.transform.position = target.transform.position;
        Owner.transform.position -= new Vector3(0, 0, 1f);

        var sighted = Owner.GetTrait<Sighted>();

        if (sighted != null)
        {
            sighted.See();
        }
    }

    private List<HexCell> GetReachableCells()
    {
        return Pathfinder.GetReachableCells(Owner.Location, Owner.ActionsAvailable);
    }

    private static List<HexCell> DiscoverReachableCells(Actor actor)
    {
        return actor.GetTrait<Mobile>().GetReachableCells();
    }

    private static int MoveToCell(Actor actor, HexCell target)
    {
        return actor.GetTrait<Mobile>().MoveToCell(target);
    }
}