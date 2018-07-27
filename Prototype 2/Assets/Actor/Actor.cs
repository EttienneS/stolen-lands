using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Actor : MonoBehaviour
{
    public Color Color;

    public List<HexCell> ControlledCells = new List<HexCell>();

    public string Name;

    public Actor(string name, Color color)
    {
        Name = name;
        Color = color;

        // resolution of sprite
        var res = 16;
        Sprite = Sprite.Create(TextureCreator.GetTexture(null, res, color), new Rect(new Vector2(), new Vector2(res, res)), new Vector2());
    }

    public Sprite Sprite;


}