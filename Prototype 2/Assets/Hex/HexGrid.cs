using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public Text cellLabelPrefab;

    public HexCell cellPrefab;

    private HexCell[] cells;

    public Color defaultColor = Color.green;

    private Canvas gridCanvas;

    private HexMesh hexMesh;

    public Tree treePrefab;

    [Range(1, 150)] public int height = 20;

    [Range(1, 150)] public int width = 20;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, y, i++);
            }
        }
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    private void CreateCell(int x, int y, int i)
    {
        float xpos = x;
        float ypos = y;

        var position = new Vector2((xpos + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f),
            ypos * (HexMetrics.outerRadius * 1.5f));

        var cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        cell.color = defaultColor;

        for (var t = 0; t < Random.Range(0, 10); t++)
        {
            if (t > 2)
            {
                var scale = Random.Range(3, 8);
                var tree = Instantiate(treePrefab);
                tree.transform.SetParent(cell.transform);

                tree.transform.localScale = new Vector3(scale, scale, scale);
                tree.transform.localPosition =
                    new Vector3(Random.Range(-10f, 10f), scale * 0.5f, Random.Range(-10f, 10f));
                tree.transform.Rotate(0, 0, Random.Range(0, 360));
            }
        }

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }

        if (y > 0)
        {
            if ((y & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);

                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        var label = Instantiate(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = position;
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    public HexCell GetCellAtPoint(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        var index = coordinates.X + coordinates.Y * width + coordinates.Y / 2;
        var cell = cells[index];


        return cell;
    }

    public void ColorCell(HexCell cell, Color color)
    {
        cell.color = color;
        hexMesh.Triangulate(cells);
    }

    public void DeselectCell(HexCell cell)
    {
    }

    
}