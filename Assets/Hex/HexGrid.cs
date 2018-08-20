using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    private static HexGrid _instance;

    public Text cellLabelPrefab;

    public HexCell cellPrefab;

    private HexCell[] cells;

    private Canvas gridCanvas;

    [Range(1, 250)] public int height = 2;

    [Range(1, 250)] public int width = 2;

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
        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[height * width];

        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, y, i++);
            }
        }

        // add all actors to the world
        var allocatedCells = new List<HexCell>();

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

                // take a few turns quickly to establish sentient actors
                var sentient = actor.GetTrait<Sentient>();
                if (sentient != null)
                {
                    for (int i = 0; i < sentient.Mental; i++)
                    {
                        actor.TakeTurn();
                    }
                }
            }

            AddActorToCanvas(actor, actor.Location);
        }
    }

    private void Start()
    {
        foreach (var cell in cells)
        {
        }
    }

    public HexCell GetRandomCell()
    {
        return cells[Random.Range(0, height * width)];
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
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);

                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
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

        cell.ColorCell(Color.grey);
    }

    public HexCell GetCellAtPoint(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        var index = coordinates.X + coordinates.Y * width + coordinates.Y / 2;
        var cell = cells[index];

        return cell;
    }
}