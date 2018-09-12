using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoodadController : MonoBehaviour
{
    private static DoodadController _instance;

    public List<GameObject> DoodadPrefabs;

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

    public GameObject CreateDoodadInCell(string doodadName, HexCell location)
    {
        var doodad = Instantiate(DoodadPrefabs.FirstOrDefault(d => d.name == doodadName), location.transform);

        location.MoveGameObjectToCell(doodad);
        location.Doodads.Add(doodad);

        return doodad;
    }
}
