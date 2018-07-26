using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Actor : MonoBehaviour
{
    public Color Color;

    public List<HexCell> ControlledCells;

    public string Name;


    private Sprite _sprite;

    public Sprite PlayerSprite
    {
        get
        {
            if (_sprite == null)
            {
                var resolution = 16;
                _sprite = Sprite.Create(TextureCreator.GetTexture(transform, resolution), new Rect(new Vector2(), new Vector2(resolution, resolution)), new Vector2());
            }

            return _sprite;
        }
    }

    public void Awake()
    {
        ControlledCells = new List<HexCell>();
    }
}