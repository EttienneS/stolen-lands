using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    private static HexGrid _instance;

    public Text cellLabelPrefab;

    public HexCell cellPrefab;

    private HexCell[] cells;

    private Canvas gridCanvas;

    [Range(1, 250)] public int Height = 2;

    [Range(1, 250)] public int Width = 2;


    [Range(1, 1000)] public int Masses = 50;

    [Range(1, 500)] public int MinMassSize = 50;

    [Range(1, 500)] public int MaxMassSize = 100;

    public static HexGrid Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
            }

            return _instance;
        }
    }

    public void AddActorToCanvas(Actor actor, HexCell cell)
    {
        var display = ActorController.Instance.GetDisplayForActor(actor);
        display.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
        display.transform.localPosition = new Vector3(cell.Label.transform.localPosition.x,
            cell.Label.transform.localPosition.y, -1f);
        display.transform.SetParent(cell.Label.transform);
    }

    private void Awake()
    {
        ActorController.Instance.Init();

        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[Height * Width];

        for (int y = 0, i = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                CreateCell(x, y, i++);
            }
        }

        GenerateMap(Masses, MinMassSize, MaxMassSize);

        // add all actors to the world
        var allocatedCells = new List<HexCell>();
        allocatedCells.AddRange(cells.Where(c => c.Height == 0).ToList());

        foreach (var actor in ActorController.Instance.Actors)
        {
            var controlled = actor.GetTrait<Controlled>();
            var controller = actor.GetTrait<Controller>();

            var cell = GetRandomCell();
            while (allocatedCells.Contains(cell))
            {
                cell = GetRandomCell();
            }

            if (controlled != null)
            {
                // actor under direct control
                continue;
            }

            if (controlled == null && controller == null)
            {
                // outlier actor
                actor.Location = cell;
            }
            else
            {
                // actor that controls others
                allocatedCells.Add(cell);
                actor.Location = cell;

                if (controller != null)
                {
                    foreach (var underling in controller.UnderControl)
                    {
                        underling.Owner.Location = actor.Location;
                    }
                }

                // take a setup turn to establish actors
                actor.TakeTurn();
            }

            AddActorToCanvas(actor, actor.Location);
        }
    }

    private void GenerateMap(int massCount, int massSizeMin, int massSizeMax)
    {
        for (int i = 0; i < massCount; i++)
        {
            var massSize = Random.Range(massSizeMin, massSizeMax);
            //GetMass(TextureHelper.GetRandomColor(), massSize);
            var massColor = new Color(0, Random.Range(0.5f, 1f), 0);

            foreach (var cell in GetMass(massSize))
            {
                cell.Height = 1;
                cell.ColorCell(massColor);
            }
        }
    }

    private List<HexCell> GetMass(int massSize)
    {
        var mass = new List<HexCell>();

        var massCenter = GetRandomCell();
        mass.Add(massCenter);

        for (int x = 0; x < massSize; x++)
        {
            var newCenter = GetRandomNeighbourNotInMass(massCenter, mass);

            if (newCenter == null)
            {
                break;
            }

            mass.Add(newCenter);

            for (int z = 0; z < Random.Range(0, 3); z++)
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

    private static HexCell GetRandomNeighbourNotInMass(HexCell cell, List<HexCell> mass)
    {
        var openCells = cell.neighbors.Where(c => c != null && !mass.Contains(c)).ToList();
        HexCell newCell = null;

        if (openCells.Any())
        {
            newCell = openCells[Random.Range(0, openCells.Count() - 1)];
        }

        return newCell;
    }



    public HexCell GetRandomCell()
    {
        return cells[Random.Range(0, Height * Width)];
    }

    private void CreateCell(int x, int y, int i)
    {
        float xpos = x;
        float ypos = y;

        var position = new Vector2((xpos + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f),
            ypos * (HexMetrics.outerRadius * 1.5f));

        var cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        cell.name = cell.coordinates + " Cell";

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }

        if (y > 0)
        {
            if ((y & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - Width]);

                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - Width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - Width]);
                if (x < Width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - Width + 1]);
                }
            }
        }

        var label = Instantiate(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = position;
        //label.text = cell.coordinates.ToStringOnSeparateLines();
        label.text = "";
        label.name = cell.coordinates + " Label";
        cell.Label = label;

        cell.ColorCell(Color.blue);
    }


    public HexCell GetCellAtPoint(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        var index = coordinates.X + coordinates.Y * Width + coordinates.Y / 2;
        var cell = cells[index];

        return cell;
    }
}