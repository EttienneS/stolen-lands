using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public void Load(string location)
    {
        var path = Path.Combine(location, "map.data");

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            // get the version of the file
            // var version = 
            reader.ReadInt32();
            Cells = new HexCell[reader.ReadInt32()];

            for (var i = 0; i < Cells.Length; i++)
            {
                HexCell.Load(i, reader);
            }
        }
    }

    public void Save(string location)
    {
        var path = Path.Combine(location, "map.data");

        using (var writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            // write the file version
            writer.Write(0);

            // write the metadata to know how many cells to load
            writer.Write(Cells.Length);

            foreach (var cell in Cells)
            {
                cell.Save(writer);
            }

            writer.Close();
        }
    }
}