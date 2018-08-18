using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{
    public Color Color;

    public List<HexCell> ControlledCells = new List<HexCell>();

    public string Name;

    public Sprite Sprite;

    [Range(0, 100)]
    public int Physical;

    [Range(0, 100)]
    public int Cunning;

    [Range(0, 100)]
    public int Mental;

    [Range(0, 100)]
    public int Charisma;

    [Range(-100, 100)]
    public int Morality;

    [Range(-100, 100)]
    public int Lawfulness;

    public HexCell Location;

    public List<ActorAction> AvailableActions = new List<ActorAction>();

    private GameObject _border;

    public void Instantiate(string name, Color color)
    {
        if (Cunning > 60)
        {
            AvailableActions.Add(ClaimCell.Agressive);            
        }
        else if (Cunning < 40)
        {
            AvailableActions.Add(ClaimCell.Cautious);            
        }
        else
        {
            AvailableActions.Add(ClaimCell.Default);            
        }

        Name = name;
        Color = color;

        // resolution of sprite
        var res = 16;
        Sprite = Sprite.Create(TextureCreator.GetTexture(null, res, color), new Rect(new Vector2(), new Vector2(res, res)), new Vector2());
    }

    public void TakeTurn()
    {
        if (AvailableActions.Any())
        {
            AvailableActions[UnityEngine.Random.Range(0, AvailableActions.Count - 1)].Execute(this);
        }
    }

    public void UpdateBorder()
    {
        if (_border != null)
        {
            Destroy(_border, 0f);
        }

        _border = new GameObject("border " + name);
        _border.transform.SetParent(SystemController.Instance.GridCanvas.transform);
        
        var width = 0.6f;
        var borderOffset = 0.3f;
        var points = new List<KeyValuePair<Vector3,Vector3>>();
        foreach (var cell in ControlledCells)
        {
            var face = 0;
            
            foreach (var neighbor in cell.neighbors)
            {
                if (neighbor == null || neighbor.Owner != this)
                {
                    var startPoint = cell.transform.position;
                    var face1 = startPoint + new Vector3(HexMetrics.corners[face].x, HexMetrics.corners[face].y, -1);
                    var face2 = startPoint + new Vector3(HexMetrics.corners[face+1].x, HexMetrics.corners[face+1].y, -1);

                    face1 = Vector3.MoveTowards(face1, startPoint, borderOffset);
                    face2 = Vector3.MoveTowards(face2, startPoint, borderOffset);

                    points.Add(new KeyValuePair<Vector3, Vector3>(face1,face2));
                }
                face++;
            }
        }

       _border.transform.localPosition = transform.position;
       _border.transform.SetParent(_border.transform);

        foreach (var point in points)
        {
            var borderLine = new GameObject("BorderLine");

            borderLine.transform.localPosition = transform.position;
            borderLine.transform.SetParent(_border.transform);

            borderLine.AddComponent<LineRenderer>();
            var lr = borderLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = Color;
            lr.endColor = Color;
            lr.startWidth = width;
            lr.endWidth = lr.startWidth;
            lr.positionCount = 2;

            lr.SetPosition(0, point.Key);
            lr.SetPosition(1, point.Value);
        }
    }

}