using System.Linq;
using UnityEngine;

public class CityCenter : Structure
{
    public int DemenseSize = 3;

    private GameObject _border;

    public override void Build()
    {
        foreach (var cell in HexGrid.Instance.GetCellsInRadiusAround(Location, DemenseSize).ToList())
        {
            Faction.Claim(cell);
        }
    }

    public override void EndTurn()
    {
    }

    public override void Init()
    {
        AddTrait(new Sighted(3));
        RotateOnZ();
    }

    public override void RevertEffect()
    {
        RevertDestroyDoodadHighlight();

        if (_border != null)
        {
            Destroy(_border);
        }
    }

    public override void ShowEffect(Entity entity)
    {
        HighlightDoodadsThatWillBeDestroyed(entity);

        if (_border != null)
        {
            Destroy(_border);
        }

        _border = GameHelpers.DrawBorder(entity.Location,
            HexGrid.Instance.GetCellsInRadiusAround(entity.Location, DemenseSize).ToList(),
            ActorController.Instance.PlayerFaction.Color,
            0.5f);
    }

    public override void TakeTurn()
    {
    }
}
