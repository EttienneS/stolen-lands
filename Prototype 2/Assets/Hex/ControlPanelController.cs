using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    public Text coordinateLabel;

    public HexController hexController;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hexController.SelectedCell != null)
        {
            coordinateLabel.text = hexController.SelectedCell.coordinates.ToString();
        }
    }
}
