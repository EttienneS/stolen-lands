using System.Collections.Generic;

public class Sentient : Trait
{
    public int Charisma;
    public int Cunning;
    public int Lawfulness;
    public int Mental;
    public int Morality;
    public int Physical;

    public Sentient(Actor owner) : base(owner)
    {
    }


    public override List<ActorAction> GetActions()
    {
        return new List<ActorAction>();
    }
}