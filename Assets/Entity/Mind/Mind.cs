using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Mind
{
    public Entity Entity;

    public Faction Faction => Entity.Faction;

    public int Charisma;
    public int Cunning;
    public int Lawfulness;
    public int Mental;
    public int Morality;
    public int Physical;

    protected Mind()
    {
        Physical = Random.Range(20, 80);
        Cunning = Random.Range(20, 80);
        Mental = Random.Range(20, 80);
        Charisma = Random.Range(20, 80);
    }

    public abstract void Act();
}