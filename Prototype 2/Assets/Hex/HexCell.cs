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

    public void DrawBorder(HexDirection[] faces)
    {
        if (faces == null || faces.Length == 0)
        {
            faces = new HexDirection[6]
            {
                HexDirection.NE, HexDirection.E, HexDirection.SE, HexDirection.SW, HexDirection.W, HexDirection.NW
            };
        }


        var start = transform.localPosition + new Vector3(0, 0, -2);
        var points = new List<KeyValuePair<Vector3, Vector3>>();
        foreach (var face in faces)
        {
            var point1 = new Vector3();
            var point2 = new Vector3();

            switch (face)
            {
                case HexDirection.NE:
                    // north
                    point1 = start + new Vector3(0f, HexMetrics.outerRadius);

                    // north-east
                    point2 = start + new Vector3(HexMetrics.innerRadius, 0.5f * HexMetrics.outerRadius);
                    break;
                case HexDirection.E:
                    break;
                case HexDirection.SE:
                    break;
                case HexDirection.SW:
                    break;
                case HexDirection.W:
                    break;
                case HexDirection.NW:
                    break;

            }

            points.Add(new KeyValuePair<Vector3, Vector3>(point1, point2));
        }

        var selectionIndicator = new GameObject { name = "Selection" };

        foreach (var point in points)
        {
            var holder = new GameObject();

            holder.transform.localPosition = transform.position;
            holder.transform.SetParent(selectionIndicator.transform);

            holder.AddComponent<LineRenderer>();
            var lr = holder.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = Color.black;
            lr.endColor = Color.black;
            lr.startWidth = 0.5f;
            lr.endWidth = 0.5f;
            lr.positionCount = 2;

            lr.SetPosition(0, point.Key);
            lr.SetPosition(1, point.Value);
        }

        //Destroy(selectionIndicator, 1f);
    }
}