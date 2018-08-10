﻿using UnityEngine;
using UnityEngine.EventSystems;

public class SystemController : MonoBehaviour
{
    private HexCell _selectedCell;

    public HexCell SelectedCell { get; set; }

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
                SelectedCell.DrawBorder(Color.clear, 0);
                SelectedCell = null;
            }

            SelectedCell = HexGrid.Instance.GetCellAtPoint(hit.point);
            SelectedCell.DrawBorder(Color.black);

            var activeActor = TurnController.Instance.ActiveActor;

            //if (SelectedCell.Owner == null)
            //{
            //    InfoController.Instance.ShowInfoBox("Hex Claimed!", activeActor.Name + " claimed " + SelectedCell.coordinates);
            //}

            ClaimCell(activeActor, SelectedCell);
        }
        else
        {
            SelectedCell = null;
        }
    }

    public void ClaimCell(Actor owner, HexCell cell)
    {
        owner.ControlledCells.Add(cell);
        cell.Owner = owner;
        cell.DrawBorder(owner.Color, HexDirectionExtensions.AllFaces, HexCell.BorderType.Control);
    }
}