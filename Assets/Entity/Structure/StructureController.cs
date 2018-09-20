using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureController : MonoBehaviour
{
    public static StructureController _instance;
    public List<Structure> StructurePrefabs;

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

    public IEnumerable<Structure> AvailableBuildings(int budget)
    {
        return StructurePrefabs.Where(b => b.Cost <= budget);
    }


    public Structure Build(Entity entity, Structure buildingPrefab)
    {
        var cell = entity.Location;

        var building = Instantiate(buildingPrefab, cell.transform);

        cell.DestroyDoodads();

        entity.Faction.AddHolding(building);
        cell.AddEntity(building);

        building.Build();

        return building;
    }
}