using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DoodadController : MonoBehaviour
{
    private static DoodadController _instance;

    public List<Doodad> DoodadPrefabs;

    public static DoodadController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("DoodadController").GetComponent<DoodadController>();
            }

            return _instance;
        }
    }

    public Doodad CreateDoodadInCell(string doodadName, HexCell location)
    {
        var doodad = Instantiate(DoodadPrefabs.FirstOrDefault(d => d.name == doodadName), location.transform);

        doodad.name = doodadName;
        location.AddDoodad(doodad);

        return doodad;
    }

    public void Load(string location)
    {
        var path = Path.Combine(location, "doodad.data");

        using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            for (var i = 0; i < HexGrid.Instance.Cells.Length; i++)
            {
                var cell = reader.ReadInt32();

                if (cell != i)
                {
                    throw new InvalidDataException();
                }

                while (reader.PeekChar() != '\r')
                {
                    var doodad = reader.ReadString();
                    CreateDoodadInCell(doodad, HexGrid.Instance.Cells[cell]);
                }

                reader.ReadChar();
            }
               
            
        }
    }

    public void Save(string location)
    {
        var path = Path.Combine(location, "doodad.data");

        using (var writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            foreach (var cell in HexGrid.Instance.Cells)
            {
                writer.Write(cell.ID);
                foreach (var doodad in cell.Doodads)
                {
                    writer.Write(doodad.name);
                }
                writer.Write('\r');
            }
        }
    }
}