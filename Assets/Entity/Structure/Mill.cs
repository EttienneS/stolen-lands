using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Mill : Structure
{
    public int EffectRange = 3;

    public override void Build()
    {
       
    }

    public override void Init()
    {
        AddTrait(new Sighted(1));
        RotateOnZ();
    }

    private List<Tree> _highlightedTrees;

    public override void ShowEffect(Entity entity)
    {
        _highlightedTrees = GetAffectedTrees(entity);

        foreach (var tree in _highlightedTrees)
        {
            tree.HighLight(Color.white);
        }
    }

    private List<Tree> GetAffectedTrees(Entity entity)
    {
        var trees = new List<Tree>();
        foreach (var cell in HexGrid.Instance.GetCellsInRadiusAround(entity.Location, EffectRange))
        {
            foreach (var doodad in cell.Doodads)
            {
                var tree = doodad.GetComponent<Tree>();
                if (tree != null)
                {
                    trees.Add(tree);
                }
            }
        }

        return trees;
    }

    public override void RevertEffect()
    {
        foreach (var tree in _highlightedTrees)
        {
            if (tree != null)
            {
                tree.DisableHighLight();
            }
        }
    }

    public override void TakeTurn()
    {
    }

    public override void EndTurn()
    {
        foreach (var tree in GetAffectedTrees(this))
        {
            Faction.Gold++;
        }
    }
}
