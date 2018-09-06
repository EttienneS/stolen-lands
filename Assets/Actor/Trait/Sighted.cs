using System.Collections.Generic;
using System.Linq;

public class Sighted : Trait
{
    private readonly int _visionRange;

    private HexCell _lastViewPoint;

    private List<HexCell> _visibleCells = new List<HexCell>();

    public Sighted(Actor owner, int visionRange) : base(owner)
    {
        _visionRange = visionRange;
    }

    public List<HexCell> VisibleCells
    {
        get
        {
            if (Owner.Location == null)
            {
                return _visibleCells;
            }

            if (Owner.Location == _lastViewPoint)
            {
                // use cache
                return _visibleCells;
            }

            _lastViewPoint = Owner.Location;
            _visibleCells = new List<HexCell>();
            var center = Owner.Location;
            _visibleCells.Add(center);

            var frontier = center.neighbors.ToList();
            var done = new List<HexCell> {center};

            while (frontier.Any())
            {
                var cell = frontier.First();
                frontier.RemoveAt(0);

                if (cell != null && !done.Contains(cell))
                {
                    if (cell.coordinates.DistanceTo(Owner.Location.coordinates) <= _visionRange)
                    {
                        _visibleCells.Add(cell);
                        frontier.AddRange(cell.neighbors);
                    }
                }

                done.Add(cell);
            }

            return _visibleCells;
        }
    }

    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();
        return actions;
    }

    public override void DoPassive()
    {
        See();
    }

    public void See()
    {
        foreach (var hex in VisibleCells)
        {
            Owner.Faction.LearnHex(hex);
        }
    }
}