using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    private static HexGrid _instance;

    private readonly Dictionary<HexCell, ActionDisplay> _playerActions = new Dictionary<HexCell, ActionDisplay>();
    public ActionDisplay ActionDisplayPrefab;
    public HexCell CellPrefab;
    public HexCell[] Cells;

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

    public void AddPlayerActionToCanvas(Player player, HexCell cell, ActorAction action)
    {
        if (!ActorController.Instance.PlayerFaction.KnownHexes.Contains(cell))
        {
            // do not add actions to unknown hexes
            return;
        }

        if (!_playerActions.ContainsKey(cell))
        {
            var actionDisplay = Instantiate(ActionDisplayPrefab);
            actionDisplay.name = "ActionDisplay - " + cell.coordinates;
            actionDisplay.transform.SetParent(SystemController.Instance.GridCanvas.transform);
            actionDisplay.transform.localPosition = cell.transform.localPosition;

            actionDisplay.Context = cell;
            actionDisplay.ActionCompleted = player.ActionExecuted;

            _playerActions.Add(cell, actionDisplay);
        }

        if (_playerActions[cell].Actions.All(a => a.ActionName != action.ActionName))
        {
            _playerActions[cell].Actions.Add(action);
        }
    }

    public void ClearPlayerActions()
    {
        foreach (var action in _playerActions.Values)
        {
            Destroy(action.gameObject, 0);
        }

        _playerActions.Clear();
    }

    public HexCell GetCellAtPoint(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        var index = coordinates.X + coordinates.Y * Width + coordinates.Y / 2;
        var cell = Cells[index];

        return cell;
    }

    public HexCell GetRandomCell()
    {
        return Cells[Random.Range(0, Height * Width)];
    }

    private void Awake()
    {
        ActorController.Instance.Init();
        MapGenerator.GenerateMap(Masses, MinMassSize, MaxMassSize);

        MapGenerator.PopulateWorld();
    }
}