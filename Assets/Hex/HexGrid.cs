using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    private static HexGrid _instance;

    public HexCell CellPrefab;
    public HexCell[] Cells;

    [Range(1, 250)] public int Height = 32;
    [Range(1, 250)] public int Width = 32;

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

    public HexCell GetRandomPathableCell()
    {
        var pathableCells = Cells.Where(c => c.Type.TravelCost > 0).ToList();
        return pathableCells[Random.Range(0, pathableCells.Count)];
    }

    private void Awake()
    {
        ActorController.Instance.Init();
        MapGenerator.GenerateMap();

        MapGenerator.PopulateWorld();
    }

    public IEnumerable<HexCell> GetCellsInRadiusAround(HexCell origin, int radius)
    {
        return Cells.Where(c => c != null && c.Coordinates.DistanceTo(origin.Coordinates) <= radius);
    }
}