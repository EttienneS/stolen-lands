using System.Collections.Generic;

public class Mobile : Trait
{
    private int _moved;
    private readonly int _speed;

    public Mobile(int speed)
    {
        _speed = speed;
    }

    private int MovesLeft => _speed - _moved;

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
        entity.GetTrait<Mobile>()._moved -= points;

        return points;
    }

    private int CostToCell(Entity entity, object cell)
    {
        return Pathfinder.GetPathCost(Pathfinder.FindPath(entity.Location, cell as HexCell)) - Owner.Location.Type.TravelCost;
    }

    public override void Start()
    {
    }

    public override void Finish()
    {
        _moved = 0;
    }

    public int MoveToCell(HexCell target)
    {
        if (Owner.Location != null)
        {
            var path = Pathfinder.FindPath(Owner.Location, target);
            _moved += CostToCell(Owner, target);

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

        target.AddEntity(Owner);
        Owner.GetTrait<Sighted>()?.See();

        // hide AI players in the fog of war
        if (!(Owner.Mind is Player))
        {
            if (target.gameObject.layer == GameHelpers.KnownLayer)
            {
                Owner.gameObject.MoveToUnknownLayer();
            }
        }
    }

    private IEnumerable<HexCell> GetReachableCells()
    {
        return Pathfinder.GetReachableCells(Owner.Location, MovesLeft);
    }

    private static int MoveToCell(Entity entity, object target)
    {
        return entity.GetTrait<Mobile>().MoveToCell(target as HexCell);
    }
}