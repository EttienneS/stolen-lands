using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour
{
    public enum BorderType
    {
        Selection,
        Control
    }

    public Dictionary<BorderType, GameObject> borders;

    public Color color;
    public HexCoordinates coordinates;

    [SerializeField] public HexCell[] neighbors;

    public Text Label { get; set; }

    public Actor Owner { get; set; }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int) direction];
    }

    public void Awake()
    {
        borders = new Dictionary<BorderType, GameObject>();
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int) direction] = cell;
        cell.neighbors[(int) direction.Opposite()] = this;
    }

    public void DrawBorder(Color color, HexDirection faces = HexDirectionExtensions.AllFaces,
        BorderType type = BorderType.Selection)
    {
        GameObject border;
        if (borders.ContainsKey(type))
        {
            border = borders[type];
            borders.Remove(type);
            Destroy(border, 0f);
        }

        if (faces == 0)
        {
            return;
        }

        border = new GameObject("border-" + type);
        border.transform.SetParent(transform);

        borders.Add(type, border);

        var start = transform.localPosition + new Vector3(0, 0, -1);
        var points = new List<KeyValuePair<Vector3, Vector3>>();

        foreach (HexDirection face in Enum.GetValues(typeof(HexDirection)))
        {
            if ((faces & face) == face)
            {
                var faceValue = (int) face;
                points.Add(new KeyValuePair<Vector3, Vector3>(HexMetrics.corners[faceValue],
                    HexMetrics.corners[faceValue + 1]));
            }
        }

        //var scaleRule = type == BorderType.Selection ? 0.7f : 1f;
        var scale = new Vector3(0, 0, type == BorderType.Selection ? -0.1f : 0);

        foreach (var point in points)
        {
            var borderLine = new GameObject("BorderLine");

            borderLine.transform.localPosition = transform.position;
            borderLine.transform.SetParent(border.transform);

            borderLine.AddComponent<LineRenderer>();
            var lr = borderLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = type == BorderType.Selection ? 0.3f : 0.7f;
            lr.endWidth = lr.startWidth;
            lr.positionCount = 2;

            lr.SetPosition(0, start + point.Key + scale);
            lr.SetPosition(1, start + point.Value + scale);
        }
    }

  
}