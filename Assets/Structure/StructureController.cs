using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureController : MonoBehaviour
{
    public List<Structure> StructurePrefabs;


    public List<string> AvailableBuildings(int budget)
    {
        return StructurePrefabs.Where(b => b.Cost <= budget).Select(b => b.name).ToList();
    }

    public static StructureController _instance;

    public static StructureController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("StructureController").GetComponent<StructureController>();
            }

            return _instance;
        }
    }

    public Structure GetBuilding(string buildingName)
    {
        return StructurePrefabs.First(p => p.name == buildingName);
    }

    public Structure Build(Entity entity, string buildingName)
    {
        var cell = entity.Location;

        var building = Instantiate(GetBuilding(buildingName), cell.transform);
        building.transform.position = cell.transform.position;
        building.Location = cell;

        entity.Faction.AddHolding(building);
        cell.Entities.Add(building);
        return building;
    }
}
