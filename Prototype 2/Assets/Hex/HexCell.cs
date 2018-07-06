using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Color color;
    public HexCoordinates coordinates;

    [SerializeField] public HexCell[] neighbors;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public GameObject border;

    private void InstantiateBorder()
    {
        border = new GameObject("border");
        border.transform.SetParent(transform);
    }

    public void DrawBorder(HexDirection faces = HexDirectionExtensions.AllFaces)
    {
        Destroy(border, 0f);

        if (faces == 0)
        {
            return;
        }

        InstantiateBorder();

        var start = transform.localPosition + new Vector3(0, 0, -1);
        var points = new List<KeyValuePair<Vector3, Vector3>>();

        if ((faces & HexDirection.NE) == HexDirection.NE)
        {
            points.Add(new KeyValuePair<Vector3, Vector3>(HexMetrics.corners[0], HexMetrics.corners[1]));
        }
        if ((faces & HexDirection.E) == HexDirection.E)
        {
            points.Add(new KeyValuePair<Vector3, Vector3>(HexMetrics.corners[1], HexMetrics.corners[2]));
        }
        if ((faces & HexDirection.SE) == HexDirection.SE)
        {
            points.Add(new KeyValuePair<Vector3, Vector3>(HexMetrics.corners[2], HexMetrics.corners[3]));
        }
        if ((faces & HexDirection.NW) == HexDirection.NW)
        {
            points.Add(new KeyValuePair<Vector3, Vector3>(HexMetrics.corners[3], HexMetrics.corners[4]));
        }
        if ((faces & HexDirection.W) == HexDirection.W)
        {
            points.Add(new KeyValuePair<Vector3, Vector3>(HexMetrics.corners[4], HexMetrics.corners[5]));
        }
        if ((faces & HexDirection.SW) == HexDirection.SW)
        {
            points.Add(new KeyValuePair<Vector3, Vector3>(HexMetrics.corners[5], HexMetrics.corners[0]));
        }

        foreach (var point in points)
        {
            var borderLine = new GameObject("BorderLine");

            borderLine.transform.localPosition = transform.position;
            borderLine.transform.SetParent(border.transform);

            borderLine.AddComponent<LineRenderer>();
            var lr = borderLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = Color.black;
            lr.endColor = Color.black;
            lr.startWidth = 0.5f;
            lr.endWidth = 0.5f;
            lr.positionCount = 2;

            lr.SetPosition(0, start + point.Key);
            lr.SetPosition(1, start + point.Value);
        }
    }
}