using UnityEngine;
using UnityEngine.EventSystems;

public class HexController : MonoBehaviour
{
    private HexCell _selectedCell;
    public HexGrid hexGrid;

    private HexCell SelectedCell
    {
        get { return _selectedCell; }
        set
        {
            if (_selectedCell != null)
            {
                // revert the previously selected cell to its original state
                hexGrid.DeselectCell(_selectedCell);
            }

            _selectedCell = value;
        }
    }


    private void Awake()
    {
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
                SelectedCell = null;
            }

            hexGrid.GetCellAtPoint(hit.point).DrawBorder(null);
        }
        else
        {
            SelectedCell = null;
        }
    }
}