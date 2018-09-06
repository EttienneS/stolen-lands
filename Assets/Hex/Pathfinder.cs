﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{
    private static HexCellPriorityQueue _searchFrontier;
    private static int _searchFrontierPhase;
    private static List<HexCell> _displayedPath;

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

    public static void ShowPath(List<HexCell> path)
    {
        ClearPath();

        if (_displayedPath != null)
        {
            foreach (var cell in path)
            {
                cell.EnableHighlight(Color.white);
            }

            path.First().EnableHighlight(Color.blue);
            path.Last().EnableHighlight(Color.red);
        }
    }

    public static void ClearPath()
    {
        if (_displayedPath != null)
        {
            foreach (var cell in _displayedPath)
            {
                cell.DisableHighlight();
            }
        }
    }

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

                if (neighbor.Height <= 0)
                {
                    continue;
                }

                var distance = current.Distance + neighbor.TravelCost;
                if (neighbor.SearchPhase < _searchFrontierPhase)
                {
                    neighbor.SearchPhase = _searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);
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

    public static List<HexCell> GetReachableCells(HexCell actorLocation, int speed)
    {
        var reachableCells = new List<HexCell>();
        var availableCells = actorLocation.neighbors.Where(n => n != null).ToList();
        var processedCells = new List<HexCell> {actorLocation};

        while (availableCells.Any())
        {
            var cell = availableCells[0];
            availableCells.RemoveAt(0);
            processedCells.Add(cell);

            var path = FindPath(actorLocation, cell);
            var pathCost = GetPathCost(path) - actorLocation.TravelCost;

            if (path.Count > 0 && pathCost <= speed)
            {
                reachableCells.Add(cell);

                foreach (var neighbor in cell.neighbors)
                {
                    if (neighbor != null && !processedCells.Contains(neighbor))
                    {
                        availableCells.Add(neighbor);
                    }
                }
            }
        }

        return reachableCells.Distinct().ToList();
    }

    public static int GetPathCost(List<HexCell> path)
    {
        var cost = 0;

        foreach (var cell in path)
        {
            if (cell != null)
            {
                cost += cell.TravelCost;
            }
        }

        return cost;
    }

    public static HexCell GetClosestOpenCell(HexCell origin)
    {
        return GetReachableCells(origin, 1).FirstOrDefault();
    }
}