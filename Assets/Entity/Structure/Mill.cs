using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Mill : Structure
{
    public int EffectRange = 3;

    public override void Init()
    {
        AddTrait(new Sighted(1));
        RotateOnZ();
    }

    private IEnumerable<HexCell> _affectCells;

    public override void ShowEffect(Entity entity)
    {
        _affectCells = HexGrid.Instance.GetCellsInRadiusAround(entity.Location, EffectRange);
        foreach (var cell in _affectCells)
        {
            foreach (var doodad in cell.Doodads)
            {
                var tree = doodad.GetComponent<Tree>();
                if (tree != null)
                {
                    tree.HighLight(Color.white);
                }
            }
        }
    }

    public override void RevertEffect()
    {
        foreach (var cell in _affectCells)
        {
            foreach (var doodad in cell.Doodads)
            {
                var tree = doodad.GetComponent<Tree>();
                if (tree != null)
                {
                    tree.DisableHighLight();
                }
            }
        }
    }
}
