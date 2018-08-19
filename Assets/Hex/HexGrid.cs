using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    private static HexGrid _instance;

    public Text cellLabelPrefab;

    public HexCell cellPrefab;

    private HexCell[] cells;

    private Canvas gridCanvas;

    [Range(1, 150)] public int height = 20;

    private HexMesh hexMesh;

    [Range(1, 150)] public int width = 20;

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
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, y, i++);
            }
        }

        foreach (var actor in ActorController.Instance.Actors)
        {
            actor.Location = GetRandomCell();

            var hexClaimer = actor.GetTrait<HexClaimer>();

            if (hexClaimer != null)
            {
                hexClaimer.Claim(actor.Location);
            }

            AddActorToCanvas(actor, actor.Location);
            var sentient = actor.GetTrait<Sentient>();
            if (sentient != null)
            {
                // take a few turns quickly to establish actors
                for (int i = 0; i < sentient.Mental / 10; i++)
                {
                    actor.TakeTurn();
                }
            }
        }
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
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
        cell.color = Color.gray;
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
        label.text = cell.coordinates.ToStringOnSeparateLines();
        label.name = cell.coordinates + " Label";

        cell.Label = label;
    }

    public HexCell GetCellAtPoint(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        var index = coordinates.X + coordinates.Y * width + coordinates.Y / 2;
        var cell = cells[index];

        return cell;
    }

    public void ColorCell(HexCell cell, Color color)
    {
        cell.color = color;
        hexMesh.Triangulate(cells);
    }
}