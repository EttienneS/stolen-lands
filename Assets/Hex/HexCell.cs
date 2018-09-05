﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour
{
    public Color Color;

    private List<Color> colors;
    public HexCoordinates coordinates;

    private int distance;
    public int Height = 0;
    private Mesh hexMesh;
    private MeshCollider meshCollider;

    [SerializeField] public HexCell[] neighbors;

    public int TravelCost;

    private List<int> triangles;
    private List<Vector3> vertices;

    public HexCell PathFrom { get; set; }
    public int SearchHeuristic { get; set; }

    public HexCell NextWithSamePriority { get; set; }

    public int SearchPhase { get; set; }

    public int SearchPriority
    {
        get
        {
            return distance + SearchHeuristic;
        }
    }

    public int Distance
    {
        get { return distance; }
        set
        {
            distance = value;
        }
    }

    public Actor Owner { get; set; }
    public Text Label { get; set; }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    public void Triangulate()
    {
        hexMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();

        var center = new Vector2(0, 0);
        for (var i = 0; i < 6; i++)
        {
            AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i + 1]);
            AddTriangleColor(Color);
        }

        hexMesh.vertices = vertices.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
    }

    private void AddTriangle(Vector2 v1, Vector2 v2, Vector2 v3)
    {
        var vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    private void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    public void ColorCell(Color color)
    {
        Color = color;
        Triangulate();
    }

    private void SetLabel(string message)
    {
        Label.text = message;
    }

    public void DisableHighlight()
    {
        var highlight = transform.Find("Highlight").GetComponent<SpriteRenderer>();
        highlight.enabled = false;
    }


    public void EnableHighlight(Color color)
    {
        var highlight = transform.Find("Highlight").GetComponent<SpriteRenderer>();
        highlight.color = color;
        highlight.enabled = true;
    }

    public bool Visble
    {
        get { return transform.Find("Overlay").GetComponent<SpriteRenderer>().enabled; }
        set { transform.Find("Overlay").GetComponent<SpriteRenderer>().enabled = !value; }
    }

    public bool Known
    {
        get { return transform.GetComponent<MeshRenderer>().enabled; }
        set { transform.GetComponent<MeshRenderer>().enabled = value; }
    }
}