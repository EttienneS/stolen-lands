﻿using System.Collections.Generic;
using UnityEngine;

public class InfoController : MonoBehaviour
{
    private readonly List<InfoBox> _boxes = new List<InfoBox>();
    public InfoBox InfoBoxPrefab;

    private static InfoController _infoController;

    public static InfoController InfoControllerInstance
    {
        get
        {
            if (_infoController == null)
            {
                _infoController = GameObject.Find("InfoService").GetComponent<InfoController>();
            }

            return _infoController;
        }
    }

    public void ShowInfoBox(string title, string body, Vector2? position = null)
    {
        var box = Instantiate(InfoBoxPrefab);
        box.SetText(title, body);
        box.transform.SetParent(transform, false);

        if (position.HasValue)
        {
            box.transform.position = position.Value;
        }
        else
        {
            box.transform.position = new Vector2(box.transform.position.x, box.transform.position.y - _boxes.Count * 30);
        }

        _boxes.Add(box);
    }

    public void Update()
    {
        // purge closed boxes from the list
        _boxes.RemoveAll(b => b == null);
    }
}