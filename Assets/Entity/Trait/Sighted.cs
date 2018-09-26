using System.Collections.Generic;
using System.Linq;

public class Sighted : Trait
{
    public int VisionRange;

    private HexCell _lastViewPoint;

    private List<HexCell> _visibleCells = new List<HexCell>();
    public Sighted() { }

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

    public override string Save()
    {
        var data = VisionRange + "|";

        foreach (var cell in VisibleCells)
        {
            data += cell.ID + ",";
        }

        return data.Trim(',');
    }

    public override void Load(string data)
    {
        var parts = data.Split('|');

        VisionRange = int.Parse(parts[0]);

        foreach (var cellId in parts[1].Split(','))
        {
            _visibleCells.Add(HexGrid.Instance.Cells[int.Parse(cellId)]);
        }
    }

    public void See()
    {
        foreach (var hex in VisibleCells)
        {
            Owner.Faction.LearnHex(hex);
        }
    }
}