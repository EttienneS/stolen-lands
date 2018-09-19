using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : Entity
{
    private List<Doodad> _destroyedDoodads;
    public int Cost;

    public abstract void Init();

    private void Start()
    {
        Init();
        StartTurn();
    }

    public override void StartTurn()
    {
        foreach (var trait in Traits)
        {
            trait.Start();
        }
    }

    public override void TakeTurn()
    {
    }

    public override void EndTurn()
    {
    }

    public void RotateOnY()
    {
        transform.localEulerAngles += new Vector3(0, Random.Range(0, 180), 0);
    }

    public void RotateOnX()
    {
        transform.localEulerAngles += new Vector3(Random.Range(0, 180), 0, 0);
    }

    public void RotateOnZ()
    {
        transform.localEulerAngles += new Vector3(0, 0, Random.Range(0, 180));
    }

    public abstract void ShowEffect(Entity entity);

    public abstract void RevertEffect();

    public override string ToString()
    {
        return name;
    }

    protected void RevertDestroyDoodadHighlight()
    {
        if (_destroyedDoodads != null)
        {
            foreach (var doodad in _destroyedDoodads)
            {
                doodad.DisableHighLight();
            }
        }
    }


    protected void HighlightDoodadsThatWillBeDestroyed(Entity entity)
    {
        _destroyedDoodads = entity.Location.Doodads;
        foreach (var doodad in _destroyedDoodads)
        {
            doodad.HighLight(Color.red);
        }
    }
}