using UnityEngine;
using UnityEngine.EventSystems;

public class HexController : MonoBehaviour
{
    public HexGrid hexGrid;

    private HexCell _selectedCell;
    private HexCell SelectedCell
    {
        get { return _selectedCell; }
        set
        {
            if (_selectedCell != null)
            {
                // revert the previously selected cell to its original state
                hexGrid.ColorCell(_selectedCell, hexGrid.defaultColor);
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

            SelectedCell = hexGrid.GetCellAtPoint(hit.point);
            hexGrid.ColorCell(SelectedCell, Color.red);
        }
        else
        {
            SelectedCell = null;
        }
    }
}