using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Tower : Structure
{
    public int EffectRange = 5;
    public override void Init()
    {
        AddTrait(new Sighted(EffectRange));
    }

    private GameObject _border;

    public override void ShowEffect(Entity entity)
    {
        HighlightDoodadsThatWillBeDestroyed(entity);

        if (_border != null)
        {
            Destroy(_border);
        }

        _border = GameHelpers.DrawBorder(entity.Location, 
                                         HexGrid.Instance.GetCellsInRadiusAround(entity.Location, EffectRange).ToList(), 
                                         Color.yellow,
                                         0.5f);
    }

    public override void RevertEffect()
    {
        RevertDestroyDoodadHighlight();

        if (_border != null)
        {
            Destroy(_border);
        }
    }
}
