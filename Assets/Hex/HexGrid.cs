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

    private void Awake()
    {
        ActorController.Instance.Init();
        MapGenerator.GenerateMap();

        MapGenerator.PopulateWorld();
    }
}