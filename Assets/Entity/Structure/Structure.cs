using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : Entity
{
    public int Cost;
    private List<Doodad> _destroyedDoodads;

    public abstract void Build();

    public abstract void Init();

    public abstract void RevertEffect();

    public void RotateOnX()
    {
        transform.localEulerAngles += new Vector3(Random.Range(0, 180), 0, 0);
    }

    public void RotateOnY()
    {
        transform.localEulerAngles += new Vector3(0, Random.Range(0, 180), 0);
    }

    public void RotateOnZ()
    {
        transform.localEulerAngles += new Vector3(0, 0, Random.Range(0, 180));
    }

    public abstract void ShowEffect(Entity entity);

    public override void StartTurn()
    {
        foreach (var trait in Traits)
        {
            trait.Start();
        }
    }

    public override string ToString()
    {
        return name;
    }

    protected void HighlightDoodadsThatWillBeDestroyed(Entity entity)
    {
        _destroyedDoodads = entity.Location.Doodads;
        foreach (var doodad in _destroyedDoodads)
        {
            doodad.HighLight(Color.red);
        }
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

    private void Start()
    {
        Init();
        StartTurn();
    }
}