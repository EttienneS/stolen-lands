using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanelController : MonoBehaviour
{
    private Player _player;

    private static TopPanelController _instance;
    public static TopPanelController Instance
    {
        get
        {

            if (_instance == null)
            {
                _instance = GameObject.Find("TopPanel").GetComponent<TopPanelController>();
            }

            return _instance;
        }
    }

    public string Gold = String.Empty;

    void Update()
    {
        if (_player == null)
        {
            _player = ActorController.Instance.Player.GetTrait<Player>();
        }

        transform.Find("Available Actions").GetComponent<Text>().text = "Available Actions: " + _player.ActionsAvailable.ToString();
        transform.Find("Gold").GetComponent<Text>().text = "Gold: " + Gold;
    }
}
