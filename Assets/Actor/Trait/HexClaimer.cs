using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexClaimer : Trait
{
    private GameObject _border;

    public List<HexCell> ControlledCells = new List<HexCell>();

    public HexClaimer(Actor owner) : base(owner)
    {
    }

    public void Claim(HexCell cell)
    {
        ControlledCells.Add(cell);
        cell.Owner = Owner;
        cell.ColorCell(Owner.Color);
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
        var claimer = actor.GetTrait<HexClaimer>();

        if (claimer.ControlledCells.Any())
        {
            foreach (var controlledCell in claimer.ControlledCells)
            {
                foreach (var cell in controlledCell.neighbors)
                {
                    if (cell != null && cell.Owner == null)
                    {
                        potentialCells.Add(cell);
                    }
                }
            }
        }
        else
        {
            potentialCells.Add(actor.Location);
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

        var width = 2.0f;
        var borderOffset = 1.1f;
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
                    var face2 = startPoint + new Vector3(HexMetrics.corners[face + 1].x, HexMetrics.corners[face + 1].y, -1);

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

    public override List<ActorAction> GetActions()
    {
        var sentience = Owner.GetTrait<Sentient>();

        // if hexclaimer is not itself sentient, assume its controller is
        if (sentience == null)
        {
            sentience = Owner.GetTrait<Controlled>().Controller.Owner.GetTrait<Sentient>();
        }

        if (sentience == null)
        {
            // no brain, no action
            return new List<ActorAction>();
        }

        var actions = new List<ActorAction>();

        if (sentience.Cunning > 60)
        {
            actions.Add(new ActorAction(Owner, 1, DiscoverAvailableCells, ClaimAvailableCell));
            actions.Add(new ActorAction(Owner, 1, DiscoverMostAgressiveCells, ClaimAvailableCell));
            actions.Add(new ActorAction(Owner, 1, DiscoverMostAgressiveCells, ClaimAvailableCell));
        }
        else if (sentience.Cunning < 30)
        {
            actions.Add(new ActorAction(Owner, 1, DiscoverLeastAggressive, ClaimAvailableCell));
            actions.Add(new ActorAction(Owner, 1, DiscoverLeastAggressive, ClaimAvailableCell));
            actions.Add(new ActorAction(Owner, 1, DiscoverAvailableCells, ClaimAvailableCell));
        }
        else
        {
            actions.Add(new ActorAction(Owner, 1, DiscoverMostAgressiveCells, ClaimAvailableCell));
            actions.Add(new ActorAction(Owner, 1, DiscoverAvailableCells, ClaimAvailableCell));
            actions.Add(new ActorAction(Owner, 1, DiscoverLeastAggressive, ClaimAvailableCell));
        }

        return actions;
    }
}