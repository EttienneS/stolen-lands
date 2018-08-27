using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelController : MonoBehaviour
{
    private Player _player;

    void Update()
    {
        if (_player == null)
        {
            _player = ActorController.Instance.Player.GetTrait<Player>();
        }

        transform.Find("Available Actions").GetComponent<Text>().text = "Available Actions: " + _player.ActionsAvailable.ToString();

    }
}
