using System.Collections.Generic;

public class Faction : Actor
{
    public List<FactionMember> Members = new List<FactionMember>();

    public Actor Leader { get; set; }

    public int Gold { get; set; }

    public int Food { get; set; }

    public int Manpower { get; set; }

    public override string ToString()
    {
        return name;
    }

    public void SetLeader(Actor leader)
    {
        Controller.AddControl(leader, this);
        Leader = leader;
    }

    public void AddMember(Actor person)
    {
        Members.Add(person.AddTrait(new FactionMember(person, this)));
    }
}