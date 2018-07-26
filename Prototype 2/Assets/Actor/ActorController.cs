using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    private Actor _player;
    public List<Actor> Actors;

    public Actor Player
    {
        get
        {
            if (_player == null)
            {
                _player = Actors.First(a => a.Name == "Player");
            }

            return _player;
        }

        set { _player = value; }
    }
}