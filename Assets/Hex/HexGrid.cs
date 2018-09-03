using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    private static HexGrid _instance;

    private readonly Dictionary<HexCell, ActionDisplay> _playerActions = new Dictionary<HexCell, ActionDisplay>();

    public ActionDisplay ActionDisplayPrefab;

    public Text cellLabelPrefab;

    public HexCell cellPrefab;

    private HexCell[] cells;

    [Range(1, 250)] public int Height = 2;

    [Range(1, 1000)] public int Masses = 50;

    [Range(1, 500)] public int MaxMassSize = 100;

    [Range(1, 500)] public int MinMassSize = 50;

    [Range(1, 250)] public int Width = 2;

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
        display.transform.localPosition =
            new Vector3(cell.transform.localPosition.x, cell.transform.localPosition.y, -1f);
        display.transform.SetParent(SystemController.Instance.GridCanvas.transform);
    }

    public void ClearPlayerActions()
    {
        foreach (var action in _playerActions.Values)
        {
            Destroy(action.gameObject, 0);
        }

        _playerActions.Clear();
    }

    public void AddPlayerActionToCanvas(Player player, HexCell cell, ActorAction action)
    {
        if (!_playerActions.ContainsKey(cell))
        {
            var actionDisplay = Instantiate(ActionDisplayPrefab);
            actionDisplay.name = "ActionDisplay - " + cell.coordinates;
            actionDisplay.transform.SetParent(SystemController.Instance.GridCanvas.transform);
            actionDisplay.transform.localPosition = cell.transform.localPosition;

            actionDisplay.Context = cell;
            actionDisplay.RefreshPlayerAction = player.RefreshActions;

            _playerActions.Add(cell, actionDisplay);
        }

        if (!_playerActions[cell].Actions.Any(a => a.ActionName == action.ActionName))
        {
            _playerActions[cell].Actions.Add(action);
        }
    }

    private void Awake()
    {
        ActorController.Instance.Init();

        cells = new HexCell[Height * Width];

        for (int y = 0, i = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                CreateCell(x, y, i++);
            }
        }

        GenerateMap(Masses, MinMassSize, MaxMassSize);

        // add all actors to the world
        var allocatedCells = new List<HexCell>();
        allocatedCells.AddRange(cells.Where(c => c.Height == 0).ToList());

        foreach (var faction in ActorController.Instance.Factions)
        {
            var cell = GetRandomCell();
            while (allocatedCells.Contains(cell) || cell.Height < 0)
            {
                cell = GetRandomCell();
            }

            faction.Leader.Location = cell;
            faction.Leader.TakeTurn();

            AddActorToCanvas(faction.Leader, cell);
        }

        FindDistancesTo(ActorController.Instance.Player.Location);
    }

    private void GenerateMap(int massCount, int massSizeMin, int massSizeMax)
    {
        // nice color index chart: http://prideout.net/archive/colors.php
        // grassland
        for (var i = 0; i < massCount; i++)
        {
            var massSize = Random.Range(massSizeMin, massSizeMax);
            var rb = Random.Range(0.1f, 0.2f);
            var massColor = new Color(rb, Random.Range(0.5f, 0.8f), rb);

            foreach (var cell in GetMass(massSize))
            {
                cell.Height = 1;
                cell.ColorCell(massColor);
            }
        }

        // desert
        for (var i = 0; i < massCount / 10; i++)
        {
            var massSize = Random.Range(massSizeMin, massSizeMax);
            var massColor = new Color(1, Random.Range(0.8f, 0.95f), Random.Range(0.8f, 0.90f));

            foreach (var cell in GetMass(massSize))
            {
                cell.Height = 1;
                cell.ColorCell(massColor);
            }
        }

        foreach (var cell in cells.Where(c => c.Height == 0))
        {
            // if cell is completely surrounded with water
            // change its height and color to reflect it being deep water
            if (cell.neighbors.All(c => c == null || c.Height <= 0))
            {
                cell.Height = -1;
                cell.ColorCell(new Color(0, 0, Random.Range(0.25f, 0.5f)));
            }
        }
    }

    private List<HexCell> GetMass(int massSize)
    {
        var mass = new List<HexCell>();

        var massCenter = GetRandomCell();
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

        cell.ColorCell(new Color(0f, 0f, Random.Range(0.8f, 1.0f)));

        var label = new GameObject(cell.coordinates.ToString());
        label.transform.SetParent(SystemController.Instance.GridCanvas.transform);
        label.transform.position = cell.transform.position;

        var text = label.AddComponent<Text>();
        text.font = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.material = text.font.material;
        text.fontSize = 4;
        text.fontStyle = FontStyle.Bold;
        text.text = cell.Distance.ToString();
        text.alignment = TextAnchor.MiddleCenter;

        var rect = label.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);

        cell.Label = text;
    }

    public HexCell GetCellAtPoint(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        var index = coordinates.X + coordinates.Y * Width + coordinates.Y / 2;
        var cell = cells[index];

        return cell;
    }

    public void FindDistancesTo(HexCell cell)
    {
        StopAllCoroutines();
        StartCoroutine(Search(cell));
    }

    private IEnumerator Search(HexCell cell)
    {
        for (var i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        var delay = new WaitForSeconds(1 / 60f);

        var frontier = new List<HexCell>();
        cell.Distance = 0;
        frontier.Add(cell);

        while (frontier.Count > 0)
        {
            yield return delay;
            var current = frontier[0];
            frontier.RemoveAt(0);

            for (var d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                var neighbor = current.GetNeighbor(d);
                if (neighbor == null)
                {
                    continue;
                }

                if (neighbor.Height <= 0)
                {
                    continue;
                }

                var distance = current.Distance + neighbor.TravelCost + 1;
                if (neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = distance;
                    frontier.Add(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    neighbor.Distance = distance;
                }

                frontier.Sort((x, y) => x.Distance.CompareTo(y.Distance));
            }
        }
    }
}