﻿using System.Collections.Generic;
using System.Linq;

public static class Pathfinder
{
    private static HexCellPriorityQueue _searchFrontier;
    private static int _searchFrontierPhase;

    public static List<HexCell> FindPath(HexCell fromCell, HexCell toCell)
    {
        var path = new List<HexCell>();

        if (fromCell != null && toCell != null)
        {
            if (Search(fromCell, toCell))
            {
                var current = toCell;
                while (current != fromCell)
                {
                    current = current.PathFrom;
                    path.Add(current);
                }

                path.Add(toCell);
            }
        }

        return path;
    }

    // private static List<HexCell> _displayedPath;
    //public static void ShowPath(List<HexCell> path)
    //{
    //    ClearPath();

    //    if (_displayedPath != null)
    //    {
    //        foreach (var cell in path)
    //        {
    //            cell.EnableHighlight(Color.white);
    //        }

    //        path.First().EnableHighlight(Colors.Default);
    //        path.Last().EnableHighlight(Colors.Highlight);
    //    }
    //}

    //public static void ClearPath()
    //{
    //    if (_displayedPath != null)
    //    {
    //        foreach (var cell in _displayedPath)
    //        {
    //            cell.DisableHighlight();
    //        }
    //    }
    //}

    private static bool Search(HexCell fromCell, HexCell toCell)
    {
        _searchFrontierPhase += 2;

        if (_searchFrontier == null)
        {
            _searchFrontier = new HexCellPriorityQueue();
        }
        else
        {
            _searchFrontier.Clear();
        }

        fromCell.SearchPhase = _searchFrontierPhase;
        fromCell.Distance = 0;
        _searchFrontier.Enqueue(fromCell);

        while (_searchFrontier.Count > 0)
        {
            var current = _searchFrontier.Dequeue();
            current.SearchPhase += 1;

            if (current == toCell)
            {
                return true;
            }

            for (var d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                var neighbor = current.GetNeighbor(d);
                if (neighbor == null ||
                    neighbor.SearchPhase > _searchFrontierPhase)
                {
                    continue;
                }

                if (neighbor.Type.TravelCost < 0)
                {
                    continue;
                }

                var distance = current.Distance + neighbor.Type.TravelCost;
                if (neighbor.SearchPhase < _searchFrontierPhase)
                {
                    neighbor.SearchPhase = _searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic = neighbor.Coordinates.DistanceTo(toCell.Coordinates);
                    _searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    var oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    _searchFrontier.Change(neighbor, oldPriority);
                }
            }
        }

        return false;
    }

    public static IEnumerable<HexCell> GetReachableCells(HexCell actorLocation, int speed)
    {
        if (actorLocation == null)
        {
            return new List<HexCell>();
        }

        var reachableCells = (from cell in
                    HexGrid.Instance.Cells.Where(c =>
                        c != null && c.Coordinates.DistanceTo(actorLocation.Coordinates) <= speed)
                              let path = FindPath(actorLocation, cell)
                              let pathCost = GetPathCost(path) - actorLocation.Type.TravelCost
                              where path.Count > 0 && pathCost <= speed
                              select cell)
            .Distinct()
            .ToList();

        reachableCells.Remove(actorLocation);

        return reachableCells;
    }

    public static int GetPathCost(IEnumerable<HexCell> path)
    {
        return path.Where(cell => cell != null).Sum(cell => cell.Type.TravelCost);
    }

    public static HexCell GetClosestOpenCell(HexCell origin)
    {
        return GetReachableCells(origin, 3).FirstOrDefault();
    }

}