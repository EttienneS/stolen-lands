using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    public Text coordinateLabel;

    public SystemController hexController;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (hexController.SelectedCell != null)
        {
            coordinateLabel.text = hexController.SelectedCell.coordinates.ToString();
        }
    }
}