using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHelpers
{
    public static Renderer[] GetAllRenderersForObject(GameObject objectToCheck)
    {
        return objectToCheck.GetComponentsInChildren<Renderer>();
    }

    public static Vector3 CalculateSizeForObject(GameObject objectToMove)
    {
        var size = new Vector3();

        foreach (var renderer in GetAllRenderersForObject(objectToMove))
        {
            size += renderer.bounds.size;
        }

        return size;
    }
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour
{
    public Color Color;

    public HexCoordinates coordinates;

    public List<GameObject> Doodads = new List<GameObject>();

    public List<Entity> Entities = new List<Entity>();
    public int Height = 0;
    private Mesh hexMesh;
    private MeshCollider meshCollider;
    [SerializeField] public HexCell[] neighbors;
    public int TravelCost;
    private List<int> triangles;
    private List<Vector3> vertices;

    public int Distance { get; set; }

    public SpriteRenderer Highlight => transform.Find("Highlight").GetComponent<SpriteRenderer>();

    public bool Known
    {
        get => MeshRenderer.enabled;
        set => MeshRenderer.enabled = value;
    }

    public Text Label { get; set; }

    public MeshRenderer MeshRenderer => transform.GetComponent<MeshRenderer>();

    public HexCell NextWithSamePriority { get; set; }

    public SpriteRenderer Overlay => transform.Find("Overlay").GetComponent<SpriteRenderer>();

    public Faction Owner { get; set; }
    public HexCell PathFrom { get; set; }
    public int SearchHeuristic { get; set; }
    public int SearchPhase { get; set; }

    public List<GameObject> CellContents
    {
        get
        {
            var contents = new List<GameObject>();

            contents.AddRange(Doodads);
            Entities.ForEach(e => contents.Add(e.gameObject));

            return contents;
        }
    }

    public int SearchPriority => Distance + SearchHeuristic;

    public bool Visble
    {
        get => Overlay.enabled;
        set => Overlay.enabled = !value;
    }

    public void ColorCell(Color color)
    {
        Color = color;
        Triangulate();
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

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
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

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    private void SetLabel(string message)
    {
        Label.text = message;
    }

    public void MoveGameObjectToCell(GameObject objectToMove)
    {
        var size = GameHelpers.CalculateSizeForObject(objectToMove).z;

        objectToMove.transform.position = transform.position;
        objectToMove.transform.position -= new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), size / 2);
    }
}