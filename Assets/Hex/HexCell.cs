using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    public Text Label { get; set; }

    public MeshRenderer MeshRenderer => transform.GetComponent<MeshRenderer>();

    public HexCell NextWithSamePriority { get; set; }


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
            var leftCorner = center + HexMetrics.Corners[i];
            var rightCorner = center + HexMetrics.Corners[i + 1];
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

        Hardpoints.Add(new Vector3(0, 0));
        for (var i = 0; i < 9; i++)
        {
            var counter = 0;
            while (true)
            {
                counter++;
                var point = GetRandomPointInCell();

                var tooClose = false;
                foreach (var currentPoint in Hardpoints)
                {
                    if (Vector3.Distance(point, currentPoint) < 1f)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    Hardpoints.Add(point);
                    break;
                }

                if (counter > 50)
                {
                    break;
                }
            }
        }

        Hardpoints.RemoveAt(0);
    }

    private static Vector3 GetRandomPointInCell()
    {
        var radius = HexMetrics.InnerRadius - 0.5f;

        return new Vector3(Random.Range(-radius, radius),
            Random.Range(-radius, radius), 0);
    }

    private List<Vector3> Hardpoints = new List<Vector3>();

    private int usedHardpoints = 0;

    private void SetLabel(string message)
    {
        Label.text = message;
    }

    public void MoveGameObjectToCell(GameObject objectToMove, bool useHardpoint)
    {
        objectToMove.transform.position = transform.position;

        // cater for offset based on size and move around in the hex
        objectToMove.transform.position -= new Vector3(0, 0, GameHelpers.CalculateSizeForObject(objectToMove).z / 2);

        if (useHardpoint)
        {
            if (usedHardpoints < Hardpoints.Count)
            {
                var pos = Hardpoints[usedHardpoints];
                objectToMove.transform.position -= pos; ;
                usedHardpoints++;
            }
            else
            {
                objectToMove.transform.position -= new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
            }
        }

        objectToMove.MoveToLayer(gameObject.layer);
    }

    public void AddEntity(Entity entity)
    {
        if (entity.Location != null && entity.Location.Entities.Contains(entity))
        {
            entity.Location.Entities.Remove(entity);
        }

        if (!Entities.Contains(entity))
        {
            Entities.Add(entity);

            // actors do not use the hardpoints, they always go to the center
            MoveGameObjectToCell(entity.gameObject, !(entity is Actor));
            entity.Location = this;
        }
    }

    public void AddDoodad(GameObject doodad)
    {
        if (!Doodads.Contains(doodad))
        {
            MoveGameObjectToCell(doodad, true);
            Doodads.Add(doodad);
        }
    }

    public void MoveToLayer(int layer)
    {
        gameObject.MoveToLayer(layer);

        foreach (var content in CellContents)
        {
            content.MoveToLayer(layer);
        }
    }
}