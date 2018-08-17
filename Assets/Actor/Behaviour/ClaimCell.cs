using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public static class ClaimCell
{
    public static ActorAction Default = new ActorAction(DiscoverAvailableCells, ClaimAvailableCell);
    public static ActorAction Agressive = new ActorAction(DiscoverMostAgressiveCells, ClaimAvailableCell);
    public static ActorAction Cautious = new ActorAction(DiscoverLeastAggressive, ClaimAvailableCell);

    public static void ClaimAvailableCell(Actor actor, object target)
    {
        var potentialCells = target as List<HexCell>;
        if (potentialCells != null && potentialCells.Any())
        {
            potentialCells[Random.Range(0, potentialCells.Count -1)].Claim(actor);
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
        
        foreach (var controlledCell in actor.ControlledCells)
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
}