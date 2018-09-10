using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public List<Building> BuildingPrefabs;


    public List<string> AvailableBuildings(int budget)
    {
        return BuildingPrefabs.Where(b => b.Cost <= budget).Select(b => b.name).ToList();
    }

    public static BuildingController _instance;

    public static BuildingController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("BuildingController").GetComponent<BuildingController>();
            }

            return _instance;
        }
    }

    public Building GetBuilding(string buildingName)
    {
        return BuildingPrefabs.First(p => p.name == buildingName);
    }

    public Building Build(Actor actor, string buildingName)
    {
        var cell = actor.Location;

        var building = Instantiate(GetBuilding(buildingName), cell.transform);
        building.transform.position = cell.transform.position;

        building.Owner = actor.Faction;
        building.GetComponent<MeshRenderer>().material.color = actor.Faction.Color;

        return building;
    }
}
