using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Color Color;

    public List<HexCell> ControlledCells = new List<HexCell>();

    public string Name;

    public Sprite Sprite;

    public void Instantiate(string name, Color color)
    {
        Name = name;
        Color = color;

        // resolution of sprite
        var res = 16;
        Sprite = Sprite.Create(TextureCreator.GetTexture(null, res, color), new Rect(new Vector2(), new Vector2(res, res)), new Vector2());
    }
}