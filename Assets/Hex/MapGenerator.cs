using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class MapGenerator
{


    private static void CreateCell(int x, int y, int i)
    {
        float xpos = x;
        float ypos = y;

        var position = new Vector3((xpos + y * 0.5f - y / 2) * (HexMetrics.InnerRadius * 2f),
            ypos * (HexMetrics.OuterRadius * 1.5f), 0);

        var cell = HexGrid.Instance.Cells[i] = Object.Instantiate(HexGrid.Instance.CellPrefab);
        cell.transform.SetParent(HexGrid.Instance.transform, false);
        cell.transform.localPosition = position;
        cell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        cell.name = cell.Coordinates + " Cell";

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, HexGrid.Instance.Cells[i - 1]);
        }

        if (y > 0)
        {
            if ((y & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, HexGrid.Instance.Cells[i - HexGrid.Instance.Width]);

                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, HexGrid.Instance.Cells[i - HexGrid.Instance.Width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, HexGrid.Instance.Cells[i - HexGrid.Instance.Width]);
                if (x < HexGrid.Instance.Width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, HexGrid.Instance.Cells[i - HexGrid.Instance.Width + 1]);
                }
            }
        }

        cell.Type = MapData.HexTypes[MapData.Type.Water];
    }

    private static HexCell GetRandomNeighbourNotInMass(HexCell cell, List<HexCell> mass)
    {
        var openCells = cell.Neighbors.Where(c => c != null && !mass.Contains(c)).ToList();
        HexCell newCell = null;

        if (openCells.Any())
        {
            newCell = openCells[Random.Range(0, openCells.Count)];
        }

        return newCell;
    }

    public static void GenerateMap()
    {
        // nice color index chart: http://prideout.net/archive/colors.php
        var masses = HexGrid.Instance.Height * 3;
        var maxMassSize = HexGrid.Instance.Height * 2;
        var minMassSize = HexGrid.Instance.Height;

        HexGrid.Instance.Cells = new HexCell[HexGrid.Instance.Height * HexGrid.Instance.Width];

        for (int y = 0, i = 0; y < HexGrid.Instance.Height; y++)
        {
            for (var x = 0; x < HexGrid.Instance.Width; x++)
            {
                CreateCell(x, y, i++);
            }
        }

        var recipe = MapData.Recipes[Random.Range(0, MapData.Recipes.Count)];

        // build the map using the selected recipe
        foreach (var kvp in recipe)
        {
            var m = masses * kvp.Value;

            for (var i = 0; i < m; i++)
            {
                foreach (var cell in GetMass(Random.Range(minMassSize, maxMassSize)))
                {
                    cell.Type = MapData.HexTypes[kvp.Key];
                }
            }
        }
        
        foreach (var cell in HexGrid.Instance.Cells.Where(c => c.Type.TypeName == MapData.Type.Water))
        {
            // if cell is completely surrounded with water
            if (cell.Neighbors.All(c => c == null || c.Type.TypeName == MapData.Type.Water || c.Type.TypeName == MapData.Type.DeepWater))
            {
                cell.Type = MapData.HexTypes[MapData.Type.DeepWater];
            }
        }

        foreach (var cell in HexGrid.Instance.Cells)
        {
            cell.MoveToLayer(GameHelpers.KnownLayer);
            cell.Elevate();
            cell.AddDoodads();
        }
    }

    private static IEnumerable<HexCell> GetMass(int massSize)
    {
        var mass = new List<HexCell>();

        var massCenter = HexGrid.Instance.GetRandomCell();
        mass.Add(massCenter);

        for (var x = 0; x < massSize; x++)
        {
            var newCenter = GetRandomNeighbourNotInMass(massCenter, mass);

            if (newCenter == null)
            {
                break;
            }

            mass.Add(newCenter);

            for (var z = 0; z < Random.Range(0, 3); z++)
            {
                var jitterCell = GetRandomNeighbourNotInMass(newCenter, mass);

                if (jitterCell != null)
                {
                    mass.Add(jitterCell);
                }
            }

            massCenter = newCenter;
        }

        return mass;
    }

    public static void PopulateWorld()
    {
        // add all actors to the world
        var allocatedCells = new List<HexCell>();

        foreach (var faction in ActorController.Instance.Factions)
        {
            var origin = HexGrid.Instance.GetRandomPathableCell();

            var counter = 0;
            while (allocatedCells.Contains(origin))
            {
                counter++;
                origin = HexGrid.Instance.GetRandomPathableCell();

                if (counter > 10)
                {
                    // counters infinite loop
                    origin = null;
                    break;
                }
            }

            if (origin != null)
            {
                allocatedCells.Add(origin);

                foreach (var member in faction.Members)
                {
                    member.GetTrait<Mobile>().MoveToCell(origin);
                    member.TakeTurn();

                    origin = Pathfinder.GetClosestOpenCell(origin);
                    allocatedCells.Add(origin);
                }
            }
        }
    }
}