using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour
{
    public readonly List<GameObject> Doodads = new List<GameObject>();
    private readonly List<Vector3> _hardpoints = new List<Vector3>();
    public readonly List<Entity> Entities = new List<Entity>();
    private Mesh _hexMesh;
    private MeshCollider _meshCollider;
    private List<int> _triangles;

    private int _usedHardpoints;
    private List<Vector3> _vertices;
    public HexCoordinates Coordinates;

    [SerializeField] public readonly HexCell[] Neighbors = new HexCell[6];

    private CellType _type;

    public CellType Type
    {
        get => _type;
        set
        {
            _type = value;
            Triangulate();
        }
    }

    private IEnumerable<GameObject> CellContents
    {
        get
        {
            var contents = new List<GameObject>();

            contents.AddRange(Doodads);
            Entities.ForEach(e => contents.Add(e.gameObject));

            return contents;
        }
    }

    public int Distance { get; set; }

    public SpriteRenderer Highlight { get; private set; }

    private MeshRenderer MeshRenderer { get; set; }

    public HexCell NextWithSamePriority { get; set; }

    public HexCell PathFrom { get; set; }

    public int SearchHeuristic { private get; set; }

    public int SearchPhase { get; set; }

    public int SearchPriority => Distance + SearchHeuristic;

    public void AddDoodad(GameObject doodad)
    {
        if (!Doodads.Contains(doodad))
        {
            MoveGameObjectToCell(doodad, true);
            Doodads.Add(doodad);
        }
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
        return Neighbors[(int)direction];
    }

    private void MoveGameObjectToCell(GameObject objectToMove, bool useHardpoint)
    {
        objectToMove.transform.position = transform.position;

        // cater for offset based on size and move around in the hex
        objectToMove.transform.position -= new Vector3(0, 0, GameHelpers.CalculateSizeForObject(objectToMove).z / 2);

        if (useHardpoint)
        {
            if (_usedHardpoints < _hardpoints.Count)
            {
                var pos = _hardpoints[_usedHardpoints];
                objectToMove.transform.position -= pos;
                _usedHardpoints++;
            }
            else
            {
                objectToMove.transform.position -= new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
            }
        }

        objectToMove.MoveToLayer(gameObject.layer);
    }

    public void MoveToLayer(int layer)
    {
        gameObject.MoveToLayer(layer);

        foreach (var content in CellContents)
        {
            content.MoveToLayer(layer);
        }
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        Neighbors[(int)direction] = cell;
        cell.Neighbors[(int)direction.Opposite()] = this;
    }

    private void Triangulate()
    {
        _hexMesh.Clear();
        _vertices.Clear();
        _triangles.Clear();

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

        _hexMesh.vertices = _vertices.ToArray();
        _hexMesh.triangles = _triangles.ToArray();

        MeshRenderer.material.color = Type.Color;

        _hexMesh.RecalculateNormals();
        _meshCollider.sharedMesh = _hexMesh;
    }

    private static Vector3 GetRandomPointInCell()
    {
        const float radius = HexMetrics.InnerRadius - 0.5f;

        return new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0);
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        var vertexIndex = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
    }

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = _hexMesh = new Mesh();

        MeshRenderer = transform.GetComponent<MeshRenderer>();
        Highlight = transform.Find("Highlight").GetComponent<SpriteRenderer>();

        _meshCollider = gameObject.AddComponent<MeshCollider>();
        _hexMesh.name = "Hex Mesh";
        _vertices = new List<Vector3>();
        _triangles = new List<int>();

        _hardpoints.Add(new Vector3(0, 0));
        for (var i = 0; i < 9; i++)
        {
            var counter = 0;
            while (true)
            {
                counter++;
                var point = GetRandomPointInCell();

                var tooClose = false;
                foreach (var currentPoint in _hardpoints)
                {
                    if (Vector3.Distance(point, currentPoint) < 1f)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    _hardpoints.Add(point);
                    break;
                }

                if (counter > 50)
                {
                    break;
                }
            }
        }

        _hardpoints.RemoveAt(0);
    }

    public void Elevate()
    {
        float elevation;

        switch (Type.TypeName)
        {
            case MapData.Type.Water:
            case MapData.Type.DeepWater:
                elevation = 0;
                break;

            case MapData.Type.Mountain:

                var mountaiNeighbours = Neighbors.Where(n => n != null && n.Type.TypeName == MapData.Type.Mountain);

                elevation = Random.Range(Type.MaxElevation / 2, Type.MaxElevation);

                if (mountaiNeighbours.Count() > 4)
                {
                    elevation += Random.Range(1.5f, 2f);
                }

                break;
            default:

                elevation = Random.Range(Type.MaxElevation / 2, Type.MaxElevation);
                break;

        }

        transform.position -= new Vector3(0, 0, elevation);
    }

    public void AddDoodads()
    {
        switch (Type.TypeName)
        {
            case MapData.Type.Forest:
                for (var d = 0; d < Random.Range(1, 4); d++)
                {
                    if (Doodads.Count < 7)
                    {
                        DoodadController.Instance.CreateDoodadInCell("Tree", this);
                    }
                }

                break;
        }


    }
}