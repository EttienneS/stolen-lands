using UnityEngine;
using System.Collections.Generic;

public class Faction : Actor
{
    public Actor Leader { get; set; }

    public List<FactionMember> Members = new List<FactionMember>();


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
