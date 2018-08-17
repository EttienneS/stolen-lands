using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public List<Action> AvailableActions = new List<Action>();

    public void Instantiate(string name, Color color)
    {
        // AvailableActions.Add(SystemController.ClaimCell);            

        Name = name;
        Color = color;

        // resolution of sprite
        var res = 16;
        Sprite = Sprite.Create(TextureCreator.GetTexture(null, res, color), new Rect(new Vector2(), new Vector2(res, res)), new Vector2());
    }

    public void TakeTurn()
    {
        var potentialCells = new List<HexCell>();
       
        foreach (var controlledCell in ControlledCells)
        {
            foreach (var cell in controlledCell.neighbors)
            {
                if (cell != null && cell.Owner == null)
                {
                    potentialCells.Add(cell);
                }
            }
        }

        if (potentialCells.Any())
        {
            potentialCells[Random.Range(0, potentialCells.Count -1)].Claim(this);
        }
    }
}