using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class House : Structure
{
    public override void Build()
    {
    }

    public override void Init()
    {
        AddTrait(new Sighted(1));
        RotateOnZ();
    }

    public override void ShowEffect(Entity entity)
    {
        HighlightDoodadsThatWillBeDestroyed(entity);
    }

    public override void RevertEffect()
    {
        RevertDestroyDoodadHighlight();
    }

    public override void TakeTurn()
    {
    }

    public override void EndTurn()
    {
    }
}
