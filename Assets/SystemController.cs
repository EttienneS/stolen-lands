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
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            if (SelectedCell != null)
            {
                // remove border
                // SelectedCell.DrawBorder(Color.clear, 0);
                SelectedCell = null;
            }

            SelectedCell = HexGrid.Instance.GetCellAtPoint(hit.point);
            //SelectedCell.DrawBorder(Color.black);

            //if (SelectedCell.Owner == null)
            //{
            //    InfoController.Instance.ShowInfoBox("Hex Claimed!", activeActor.Name + " claimed " + SelectedCell.coordinates);
            //}
            // SelectedCell.Claim(activeActor);
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
}