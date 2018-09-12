using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HexClaimer : Trait
{
    private GameObject _border;


    public void Claim(HexCell cell)
    {
        Owner.Faction.ControlledCells.Add(cell);
        cell.Owner = Owner.Faction;

        UpdateBorder();
    }

    public static int ClaimCell(Entity entity, object target)
    {
        entity.GetTrait<HexClaimer>().Claim(target as HexCell);
        return 1;
    }

    public static Dictionary<int, List<HexCell>> GetHexesByThreat(Entity entity)
    {
        var cells = DiscoverAvailableCells(entity);

        // get the cell along with their neighbours
        var cellLookup = new Dictionary<int, List<HexCell>>();
        foreach (var cell in cells)
        {
            var neighbors = cell.neighbors.Count(n => n != null && n.Owner != null && n.Owner != entity.Faction);
            if (!cellLookup.ContainsKey(neighbors))
            {
                cellLookup.Add(neighbors, new List<HexCell>());
            }

            cellLookup[neighbors].Add(cell);
        }

        if (!cellLookup.Any())
        {
            cellLookup.Add(0, cells);
        }

        return cellLookup;
    }

    public static object DiscoverLeastAggressive(Entity entity)
    {
        return GetHexesByThreat(entity).First().Value;
    }

    public static object DiscoverMostAgressiveCells(Entity entity)
    {
        return GetHexesByThreat(entity).Last().Value;
    }

    public static List<HexCell> DiscoverAvailableCells(Entity entity)
    {
        var potentialCells = new List<HexCell>();
        var claimer = entity.GetTrait<HexClaimer>();

        if (claimer.Owner.Faction.ControlledCells.Any())
        {
            foreach (var controlledCell in claimer.Owner.Faction.ControlledCells)
            {
                foreach (var cell in controlledCell.neighbors)
                {
                    if (cell != null && cell.Owner == null && cell.Height > 0)
                    {
                        potentialCells.Add(cell);
                    }
                }
            }
        }
        else
        {
            potentialCells.Add(entity.Location);
        }

        return potentialCells;
    }

    public void UpdateBorder()
    {
        if (_border != null)
        {
            Object.Destroy(_border);
        }

        _border = new GameObject("border " + Owner.name);
        _border.transform.SetParent(SystemController.Instance.GridCanvas.transform);

        const float width = 0.2f;
        var height = -1f;
        var points = new List<KeyValuePair<Vector3, Vector3>>();
        foreach (var cell in Owner.Faction.ControlledCells)
        {
            var face = 0;

            foreach (var neighbor in cell.neighbors)
            {
                if (neighbor == null || neighbor.Owner != Owner.Faction)
                {
                    var startPoint = cell.transform.position;
                    var face1 = startPoint + new Vector3(HexMetrics.corners[face].x, HexMetrics.corners[face].y, -cell.transform.position.z + height);
                    var face2 = startPoint + new Vector3(HexMetrics.corners[face + 1].x, HexMetrics.corners[face + 1].y, -cell.transform.position.z + height);
                    points.Add(new KeyValuePair<Vector3, Vector3>(face1, face2));
                }

                face++;
            }
        }

        _border.transform.localPosition = Owner.transform.position;
        _border.transform.SetParent(_border.transform);

        var material = Resources.Load<Material>("Line");

        foreach (var point in points)
        {
            var borderLine = new GameObject("BorderLine");

            borderLine.transform.localPosition = new Vector3(Owner.transform.position.x, Owner.transform.position.y, 0);
            borderLine.transform.SetParent(_border.transform);

            borderLine.AddComponent<LineRenderer>();
            var lr = borderLine.GetComponent<LineRenderer>();
            lr.material = material;
            lr.startColor = Owner.Faction.Color;
            lr.endColor = Owner.Faction.Color;
            lr.startWidth = width;
            lr.endWidth = lr.startWidth;
            lr.positionCount = 2;

            lr.SetPosition(0, point.Key);
            lr.SetPosition(1, point.Value);
        }
    }

    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();

        if (Owner.Faction == ActorController.Instance.PlayerFaction)
        {
            actions.Add(new ActorAction("Claim", Owner, DiscoverAvailableCells, GetCost, ClaimCell));
        }
        else
        {
            actions.Add(new ActorAction("Claim", Owner, DiscoverAvailableCells, GetCost, ClaimCell));
            actions.Add(new ActorAction("Claim", Owner, DiscoverMostAgressiveCells, GetCost, ClaimCell));
            actions.Add(new ActorAction("Claim", Owner, DiscoverLeastAggressive, GetCost, ClaimCell));
        }

        return actions;
    }


    public int GetCost(Entity entity, object cell)
    {
        return 1;
    }

    public override void Start()
    {
    }

    public override void Finish()
    {
    }
}