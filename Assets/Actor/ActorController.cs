using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public void Awake()
    {
        AddActor("Player", Color.red);
        AddActor("Enemy 1", Color.blue);
        AddActor("Enemy 2", Color.green);
        AddActor("Enemy 3", Color.yellow);
        AddActor("Enemy 4", Color.magenta);
    }

    public Actor[] Actors
    {
        get
        {
            return GetComponentsInChildren<Actor>();
        }
    }

    public void AddActor(string name, Color color)
    {
        var actorObject = new GameObject(name);
        actorObject.transform.SetParent(transform);
        actorObject.AddComponent(typeof(Actor));
        actorObject.GetComponent<Actor>().Instantiate(name, color);
    }
}