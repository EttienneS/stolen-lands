using UnityEngine;

public abstract class Structure : Entity
{
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
}