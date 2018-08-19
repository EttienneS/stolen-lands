using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexClaimer : Trait
{
    private GameObject _border;

    public List<HexCell> ControlledCells = new List<HexCell>();

    public HexClaimer(Actor owner) : base(owner)
    {
        Actions = new List<ActorAction>
        {
            new ActorAction(DiscoverAvailableCells, ClaimAvailableCell),
            new ActorAction(DiscoverMostAgressiveCells, ClaimAvailableCell),
            new ActorAction(DiscoverLeastAggressive, ClaimAvailableCell)
        };
    }

    public void Claim(HexCell cell)
    {
        ControlledCells.Add(cell);
        cell.Owner = Owner;
        UpdateBorder();
    }

    public static void ClaimAvailableCell(Actor actor, object target)
    {
        var potentialCells = target as List<HexCell>;
        if (potentialCells != null && potentialCells.Any())
        {
            actor.GetTrait<HexClaimer>().Claim(potentialCells[Random.Range(0, potentialCells.Count - 1)]);
        }
    }

    public static Dictionary<int, List<HexCell>> GetHexesByThreat(Actor actor)
    {
        var cells = DiscoverAvailableCells(actor);

        // get the cell along with their neighbours
        var cellLookup = new Dictionary<int, List<HexCell>>();
        foreach (var cell in cells)
        {
            var neighbors = cell.neighbors.Count(n => n != null && n.Owner != null && n.Owner != actor);
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

    public static List<HexCell> DiscoverLeastAggressive(Actor actor)
    {
        return GetHexesByThreat(actor).First().Value;
    }

    public static List<HexCell> DiscoverMostAgressiveCells(Actor actor)
    {
        return GetHexesByThreat(actor).Last().Value;
    }

    public static List<HexCell> DiscoverAvailableCells(Actor actor)
    {
        var potentialCells = new List<HexCell>();

        foreach (var controlledCell in actor.GetTrait<HexClaimer>().ControlledCells)
        {
            foreach (var cell in controlledCell.neighbors)
            {
                if (cell != null && cell.Owner == null)
                {
                    potentialCells.Add(cell);
                }
            }
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

        var width = 0.6f;
        var borderOffset = 0.3f;
        var points = new List<KeyValuePair<Vector3, Vector3>>();
        foreach (var cell in ControlledCells)
        {
            var face = 0;

            foreach (var neighbor in cell.neighbors)
            {
                if (neighbor == null || neighbor.Owner != Owner)
                {
                    var startPoint = cell.transform.position;
                    var face1 = startPoint + new Vector3(HexMetrics.corners[face].x, HexMetrics.corners[face].y, -1);
                    var face2 = startPoint +
                                new Vector3(HexMetrics.corners[face + 1].x, HexMetrics.corners[face + 1].y, -1);

                    face1 = Vector3.MoveTowards(face1, startPoint, borderOffset);
                    face2 = Vector3.MoveTowards(face2, startPoint, borderOffset);

                    points.Add(new KeyValuePair<Vector3, Vector3>(face1, face2));
                }

                face++;
            }
        }

        _border.transform.localPosition = Owner.transform.position;
        _border.transform.SetParent(_border.transform);

        foreach (var point in points)
        {
            var borderLine = new GameObject("BorderLine");

            borderLine.transform.localPosition = Owner.transform.position;
            borderLine.transform.SetParent(_border.transform);

            borderLine.AddComponent<LineRenderer>();
            var lr = borderLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = Owner.Color;
            lr.endColor = Owner.Color;
            lr.startWidth = width;
            lr.endWidth = lr.startWidth;
            lr.positionCount = 2;

            lr.SetPosition(0, point.Key);
            lr.SetPosition(1, point.Value);
        }
    }
}