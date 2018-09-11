using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour
{
    public Color Color;

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

    public List<Structure> Structures { get; set; }

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

    public Faction Owner { get; set; }
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
        triangles = new List<int>();
    }

    public void Triangulate()
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();

        var center = new Vector3(0, 0);
        for (var i = 0; i < 6; i++)
        {
            var leftCorner = center + HexMetrics.corners[i];
            var rightCorner = center + HexMetrics.corners[i + 1];
            var offset = new Vector3(0, 0, 6f);
            var lowerLeft = leftCorner + offset;
            var lowerRight = rightCorner + offset;

            AddTriangle(center, leftCorner, rightCorner);
            AddTriangle(rightCorner, leftCorner, lowerLeft);
            AddTriangle(lowerLeft, lowerRight, rightCorner);
        }

        var tangent = new Vector4(1f, 0f, 0f, -1f);
        var tangents = new List<Vector4>();
        for (var i = 0; i < vertices.Count; i++)
        {
            tangents.Add(tangent);
        }

        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.tangents = tangents.ToArray();

        MeshRenderer.material.color = Color;

        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        var vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
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
        Highlight.enabled = false;
    }


    public void EnableHighlight(Color color)
    {
        Highlight.color = color;
        Highlight.enabled = true;
    }

    public SpriteRenderer Overlay
    {
        get { return transform.Find("Overlay").GetComponent<SpriteRenderer>(); }
    }

    public SpriteRenderer Highlight
    {
        get { return transform.Find("Highlight").GetComponent<SpriteRenderer>(); }
    }

    public bool Visble
    {
        get { return Overlay.enabled; }
        set { Overlay.enabled = !value; }
    }

    public MeshRenderer MeshRenderer
    {
        get { return transform.GetComponent<MeshRenderer>(); }
    }

    public bool Known
    {
        get { return MeshRenderer.enabled; }
        set { MeshRenderer.enabled = value; }
    }
}