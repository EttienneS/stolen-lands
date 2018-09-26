using UnityEngine;

public abstract class Mind
{
    public int Charisma;
    public int Cunning;
    public Entity Entity;
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

    public Faction Faction => Entity.Faction;

    public abstract void Act();

    public abstract string Save();
    public abstract void Load(string data);
}