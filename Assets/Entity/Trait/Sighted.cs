using System.Collections.Generic;
using System.Linq;

public class Sighted : Trait
{
    public readonly int VisionRange;

    private HexCell _lastViewPoint;

    private List<HexCell> _visibleCells = new List<HexCell>();

    public Sighted(int visionRange)
    {
        VisionRange = visionRange;
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
            _visibleCells = HexGrid.Instance.GetCellsInRadiusAround(center, VisionRange).ToList();
            _visibleCells.Add(center);

            return _visibleCells;
        }
    }

    public override List<ActorAction> GetActions()
    {
        return new List<ActorAction>();
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