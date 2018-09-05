﻿using UnityEngine;
using UnityEngine.UI;

public class TopPanelController : MonoBehaviour
{
    private static TopPanelController _instance;
    private Player _player;

    public string Gold = string.Empty;

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

    private void Update()
    {
        if (_player == null)
        {
            _player = ActorController.Instance.PlayerFaction.Leader.GetTrait<Player>();
        }

        if (_player != null)
        {
            transform.Find("Available Actions").GetComponent<Text>().text =
                "Available Actions: " + ActorController.Instance.PlayerFaction.Leader.ActionsAvailable;
            transform.Find("Gold").GetComponent<Text>().text = "Gold: " + Gold;
        }
    }
}