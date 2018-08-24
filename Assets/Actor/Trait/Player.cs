using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Sentient
{
    public Player(Actor owner) : base(owner)
    {
    }


    public override List<ActorAction> GetActions()
    {
        return new List<ActorAction>();
    }

    public new void TakeAction(List<ActorAction> allActions)
    {
        // do nothing, player takes its own action
    }
}