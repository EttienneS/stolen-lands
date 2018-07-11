using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Color Color;

    public List<HexCell> ControlledCells;
    public string Name;

    public void Awake()
    {
        ControlledCells = new List<HexCell>();
    }
}