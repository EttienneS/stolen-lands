using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameHelpers
{
    public static int KnownLayer => 9;
    public static int UnknownLayer => 8;
    public static int VisibleLayer => 10;


    public static Vector3 CalculateSizeForObject(GameObject objectToMove)
    {
        var size = new Vector3();

        foreach (var renderer in GetAllRenderersForObject(objectToMove))
        {
            size += renderer.bounds.size;
        }

        return size;
    }

    public static Renderer[] GetAllRenderersForObject(GameObject objectToCheck)
    {
        return objectToCheck.GetComponentsInChildren<Renderer>();
    }

    public static void MoveToKnownLayer(this GameObject item)
    {
        MoveToLayer(item, KnownLayer);
    }

    public static void MoveToLayer(this GameObject item, int visibleLayer)
    {
        item.layer = visibleLayer;

        foreach (Transform child in item.transform)
        {
            if (child == null)
            {
                continue;
            }

            MoveToLayer(child.gameObject, visibleLayer);
        }
    }

    public static void MoveToUnknownLayer(this GameObject item)
    {
        MoveToLayer(item, UnknownLayer);
    }

    public static void MoveToVisibleLayer(this GameObject item)
    {
        MoveToLayer(item, VisibleLayer);
    }

    public static GameObject DrawBorder(HexCell origin, List<HexCell> borderCells, Color color, float width = 0.2f)
    {
        var border = new GameObject("border");
        border.transform.SetParent(HexGrid.Instance.transform);
        
        var points = new List<KeyValuePair<Vector3, Vector3>>();

        foreach (var cell in borderCells)
        {
            var face = 0;
            foreach (var neighbor in cell.Neighbors)
            {
                if (neighbor == null || !borderCells.Contains(neighbor))
                {
                    var startPoint = cell.transform.position;
                    var face1 = startPoint + new Vector3(HexMetrics.Corners[face].x, HexMetrics.Corners[face].y, -1f);
                    var face2 = startPoint + new Vector3(HexMetrics.Corners[face + 1].x, HexMetrics.Corners[face + 1].y, -1f);
                    points.Add(new KeyValuePair<Vector3, Vector3>(face1, face2));
                }
                face++;
            }
        }

        var material = Resources.Load<Material>("Line");
        foreach (var point in points)
        {
            var borderLine = new GameObject("BorderLine");
            borderLine.transform.localPosition = origin.transform.position;
            borderLine.transform.SetParent(border.transform);
            borderLine.AddComponent<LineRenderer>();
            var lr = borderLine.GetComponent<LineRenderer>();
            lr.material = material;
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = width;
            lr.endWidth = lr.startWidth;
            lr.positionCount = 2;
            lr.SetPosition(0, point.Key);
            lr.SetPosition(1, point.Value);
        }

        MoveToVisibleLayer(border);
        return border;
    }
}