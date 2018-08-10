using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public void Awake()
    {
        for (int i = 0; i < 100; i++)
        {
            Person.GetAveragePerson(transform);
        }
    }

    public Actor[] Actors
    {
        get
        {
            return GetComponentsInChildren<Actor>();
        }
    }

}