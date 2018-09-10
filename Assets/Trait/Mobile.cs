using System.Collections.Generic;
using UnityEngine;

public class Mobile : Trait
{
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

    public int CostToCell(Entity entity, object cell)
    {
        return Pathfinder.GetPathCost(Pathfinder.FindPath(entity.Location, cell as HexCell)) - Owner.Location.TravelCost;
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
        if (target.transform == null)
        {
            return;
        }

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
        return Pathfinder.GetReachableCells(Owner.Location, Owner.ActionPoints);
    }

    private static List<HexCell> DiscoverReachableCells(Entity entity)
    {
        return entity.GetTrait<Mobile>().GetReachableCells();
    }

    private static int MoveToCell(Entity entity, object target)
    {
        return entity.GetTrait<Mobile>().MoveToCell(target as HexCell);
    }
}