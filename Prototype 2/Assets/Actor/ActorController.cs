using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class ActorController : MonoBehaviour
{
    private Actor _player;

    private List<Actor> _actors;
    public List<Actor> Actors
    {
        get
        {
            if (_actors == null)
            {
                _actors = new List<Actor>
                {
                    new Actor("Player", Color.red),
                    new Actor("Enemy 1", Color.blue),
                    new Actor("Enemy 2", Color.yellow),
                    new Actor("Enemy 3", Color.green)
                };
            }

            return _actors;
        }
    }


    public Actor Player
    {
        get
        {
            if (_player == null)
            {
                _player = Actors.First();
            }

            return _player;
        }

        set { _player = value; }
    }
}