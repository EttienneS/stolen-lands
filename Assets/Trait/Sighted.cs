using System.Collections.Generic;
using System.Linq;

public class Sighted : Trait
{
    private readonly int _visionRange;

    private HexCell _lastViewPoint;

    private List<HexCell> _visibleCells = new List<HexCell>();

    public Sighted(int visionRange) 
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
            var center = Owner.Location;
            _visibleCells = HexGrid.Instance.Cells.Where(c => c.coordinates.DistanceTo(center.coordinates) <= _visionRange).ToList();
            _visibleCells.Add(center);

            return _visibleCells;
        }
    }

    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();
        return actions;
    }

    public override void Start()
    {
        See();
    }

    public override void Finish()
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