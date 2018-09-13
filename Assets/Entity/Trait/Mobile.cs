using System.Collections.Generic;

public class Mobile : Trait
{
    public int Moved;
    public int Speed;

    public Mobile(int speed)
    {
        Speed = speed;
    }

    public int MovesLeft => Speed - Moved;

    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();

        if (MovesLeft > 0)
        {
            foreach (var cell in GetReachableCells())
            {
                actions.Add(new ActorAction("Move (" + MovesLeft + ")", Owner, MoveToCell, cell));
            }
        }

        if (Owner.ActionPoints > 0)
        {
            actions.Add(new ActorAction("Move +", Owner, ConvertActionToMoves, Owner.ActionPoints));
        }

        return actions;
    }

    private static int ConvertActionToMoves(Entity entity, object target)
    {
        var points = int.Parse(target.ToString());
        entity.GetTrait<Mobile>().Moved -= points;

        return points;
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
        // moves instantly to location
        // use MoveToCell to move along a path

        if (target == null || target.transform == null)
        {
            return;
        }

        if (Owner.Location != null)
        {
            Owner.Location.Entities.Remove(Owner);
        }

        Owner.Location = target;
        target.Entities.Add(Owner);

        target.MoveGameObjectToCell(Owner.gameObject);

        Owner.GetTrait<Sighted>()?.See();
    }

    private List<HexCell> GetReachableCells()
    {
        return Pathfinder.GetReachableCells(Owner.Location, MovesLeft);
    }

    private static int MoveToCell(Entity entity, object target)
    {
        return entity.GetTrait<Mobile>().MoveToCell(target as HexCell);
    }
}