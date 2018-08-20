using UnityEngine;
using System.Collections.Generic;

public class Faction : Actor
{
    public Actor Leader { get; set; }

    public List<FactionMember> Members = new List<FactionMember>();


    public override string ToString()
    {
        return Name;
    }

    public void SetLeader(Actor leader)
    {
        Name = leader.Name + "'s Faction";
        Controller.AddControl(leader, this);
        Leader = leader;
    }

    public void AddMember(Actor person)
    {
        Members.Add(person.AddTrait(new FactionMember(person, this)));
    }
}
