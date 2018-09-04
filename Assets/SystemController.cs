﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour
{
    private static SystemController _instance;

    public Canvas GridCanvas;

    public Canvas UICanvas;

    public HexCell SelectedCell { get; set; }

    public static SystemController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("System").GetComponent<SystemController>();
            }

            return _instance;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) HandleInput();
    }

    private void HandleInput()
    {
        var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            if (SelectedCell != null)
            {
                // remove highlight
                SelectedCell.DisableHighlight();
                SelectedCell = null;
            }

            SelectedCell = HexGrid.Instance.GetCellAtPoint(hit.point);
        }
        else
        {
            SelectedCell = null;
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("Loading");
    }

    public Actor SelectedActor;

    public void SetSelectedActor(Actor actor)
    {
        if (SelectedActor != null)
        {
            SelectedActor.DisableOtline();
        }

        SelectedActor = actor;
        SelectedActor.EnableOutline();
        SelectedActor.TakeTurn();

        var player = actor.GetTrait<Player>();
        if (player != null)
        {
            player.TakeAction(actor.AvailableActions);
        }
    }
}