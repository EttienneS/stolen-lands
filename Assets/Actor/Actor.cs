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

    public List<ActorAction> AvailableActions = new List<ActorAction>();

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
            AvailableActions[Random.Range(0, AvailableActions.Count - 1)].Execute(this);
        }
    }
}