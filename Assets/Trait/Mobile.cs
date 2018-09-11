using System.Collections.Generic;
using UnityEngine;

public class Mobile : Trait
{
    public int Moved;
    public int Speed;

    public Mobile(int speed)
    {
        Speed = speed;
    }

    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();

        if (Owner.Faction == ActorController.Instance.PlayerFaction)
        {
            actions.Add(new ActorAction("Move", Owner, DiscoverReachableCells, CostToCell, MoveToCell));
            actions.Add(new ActorAction("+Move", Owner, DiscoverConvert, ConvertCost, ConvertActionToMoves));
        }

        return actions;
    }

    private static int ConvertActionToMoves(Entity entity, object target)
    {
        var points = int.Parse(target.ToString());
        entity.GetTrait<Mobile>().Moved -= points;

        return points;
    }

    private static int ConvertCost(Entity entity, object target)
    {
        return int.Parse(target.ToString());
    }

    private static object DiscoverConvert(Entity entity)
    {
        var options = new List<object>();
        for (var i = 0; i < entity.ActionPoints; i++)
        {
            options.Add(i + 1);
        }

        return options;
    }

    public int CostToCell(Entity entity, object cell)
    {
        return Pathfinder.GetPathCost(Pathfinder.FindPath(entity.Location, cell as HexCell)) -
               Owner.Location.TravelCost;
    }

    public override void Start()
    {
    }

    public override void Finish()
    {
        Moved = 0;
    }

    public int MoveToCell(HexCell target)
    {
        if (Owner.Location != null)
        {
            var path = Pathfinder.FindPath(Owner.Location, target);
            Moved += CostToCell(Owner, target);

            // move along path
            foreach (var cell in path)
            {
                Move(cell);
            }

            // no "cost" we subtract the moved from the current available moves
            return 0;
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
        return Pathfinder.GetReachableCells(Owner.Location, Speed - Moved);
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