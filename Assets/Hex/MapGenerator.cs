using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class MapGenerator
{
    public static void Elevate(HexCell cell, float elevation)
    {
        elevation = Mathf.Clamp(elevation, 0, 3f);
        var elevationVector = new Vector3(0, 0, elevation);
        cell.transform.position -= elevationVector;
    }

    private static void CreateCell(int x, int y, int i)
    {
        float xpos = x;
        float ypos = y;

        var position = new Vector3((xpos + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f),
            ypos * (HexMetrics.outerRadius * 1.5f), 0);

        var cell = HexGrid.Instance.Cells[i] = Object.Instantiate(HexGrid.Instance.CellPrefab);
        cell.transform.SetParent(HexGrid.Instance.transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        cell.name = cell.coordinates + " Cell";
        cell.TravelCost = 1;

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

        cell.ColorCell(new Color(0f, 0f, Random.Range(0.8f, 1.0f)));

        AddLabelToCell(cell);
    }

    private static void AddLabelToCell(HexCell cell)
    {
        var label = new GameObject(cell.coordinates.ToString());
        label.transform.SetParent(SystemController.Instance.GridCanvas.transform);
        label.transform.position = cell.transform.position;

        var text = label.AddComponent<Text>();
        text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.material = text.font.material;
        text.fontSize = 4;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;

        var rect = label.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);

        cell.Label = text;
    }

    private static HexCell GetRandomNeighbourNotInMass(HexCell cell, List<HexCell> mass)
    {
        var openCells = cell.neighbors.Where(c => c != null && !mass.Contains(c)).ToList();
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

        // desert
        for (var i = 0; i < masses / 20; i++)
        {
            var massSize = Random.Range(minMassSize, maxMassSize);
            var massColor = new Color(1, Random.Range(0.8f, 0.95f), Random.Range(0.8f, 0.90f));

            var massElevation = Random.Range(-1.5f, 0);

            foreach (var cell in GetMass(massSize))
            {
                cell.Height = 1;
                cell.TravelCost = 2;
                cell.ColorCell(massColor);

                Elevate(cell, massElevation);
            }
        }

        // grassland
        for (var i = 0; i < masses; i++)
        {
            var massSize = Random.Range(minMassSize, maxMassSize);
            var rb = Random.Range(0.1f, 0.2f);
            var massColor = new Color(rb, Random.Range(0.5f, 0.8f), rb);

            var massElevation = Random.Range(0, 1.5f);

            foreach (var cell in GetMass(massSize))
            {
                cell.Height = 1;
                cell.ColorCell(massColor);
                cell.TravelCost = 1;

                Elevate(cell, massElevation);

                if (Random.Range(0, 100) > 75)
                {
                    for (var d = 0; d < Random.Range(1, 4); d++)
                    {
                        DoodadController.Instance.CreateDoodadInCell("Tree", cell);
                    }
                }
            }
        }

        foreach (var cell in HexGrid.Instance.Cells.Where(c => c.Height == 0))
        {
            // if cell is completely surrounded with water
            // change its height and color to reflect it being deep water
            if (cell.neighbors.All(c => c == null || c.Height <= 0))
            {
                cell.Height = -1;
                cell.ColorCell(new Color(0, 0, Random.Range(0.25f, 0.5f)));
            }
        }

        foreach (var cell in HexGrid.Instance.Cells)
        {
            cell.MoveToLayer(GameHelpers.KnownLayer);
        }
    }

    private static List<HexCell> GetMass(int massSize)
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
        allocatedCells.AddRange(HexGrid.Instance.Cells.Where(c => c.Height == 0).ToList());

        foreach (var faction in ActorController.Instance.Factions)
        {
            var origin = HexGrid.Instance.GetRandomCell();
            while (allocatedCells.Contains(origin) || origin.Height < 0)
            {
                origin = HexGrid.Instance.GetRandomCell();
            }

            foreach (var member in faction.Members)
            {
                member.GetTrait<Mobile>().MoveToCell(origin);
                member.TakeTurn();

                origin = Pathfinder.GetClosestOpenCell(origin);
            }
        }
    }
}