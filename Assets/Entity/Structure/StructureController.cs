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

    public IEnumerable<Structure> AvailableBuildings(Entity builder)
    {
        var options = new List<Structure>();

        var center = StructurePrefabs.OfType<CityCenter>().First();
        if (builder.Location.Owner == builder.Faction)
        {
            options.AddRange(StructurePrefabs);
            options.Remove(center);
        }
        else if (builder.Location.Owner == null)
        {
            options.Add(center);
        }

        return options.Where(b => b.Cost <= builder.ActionPoints);
    }


    public Structure LoadBuilding(Faction owner, string buildingName, HexCell cell)
    {
        var prefab = StructurePrefabs.First(s => s.name == buildingName);
        var building = Instantiate(prefab, cell.transform);
        building.name = prefab.name;

        owner.AddHolding(building);
        cell.AddEntity(building);

        return building;
    }

    public Structure Build(Entity entity, Structure buildingPrefab)
    {
        var cell = entity.Location;

        var building = Instantiate(buildingPrefab, cell.transform);
        building.name = buildingPrefab.name;

        cell.DestroyDoodads();

        entity.Faction.AddHolding(building);
        cell.AddEntity(building);

        building.Build();

        return building;
    }
}