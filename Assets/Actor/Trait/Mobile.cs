using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mobile : Trait
{
    public Mobile(Actor owner) : base(owner)
    {
    }

    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();

        if (Owner == ActorController.Instance.Player)
        {
            actions.Add(new ActorAction("Move", Owner, 0, DiscoverReachableCells, MoveToCell));
        }
        else
        {
            // AI MOVEMENT HERE
            // actions.Add(new ActorAction("Move", Owner, 0, DiscoverReachableCells, MoveToCell));
        }

        return actions;
    }

    public void MoveToCell(HexCell target)
    {
        if (Owner.Location != null)
        {
            // if owner is nowhere, move instantly
            var path = HexGrid.Instance.FindPath(Owner.Location, target);
            Owner.ActionsAvailable -= (HexGrid.Instance.GetPathCost(path) - Owner.Location.TravelCost);
        }

        Owner.Location = target;
        Owner.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);

        foreach (var hex in target.neighbors)
        {
            var member = Owner.GetTrait<FactionMember>();

            if (member != null)
            {
                member.Faction.LearnHex(hex);
            }
        }
    }

    private List<HexCell> GetReachableCells()
    {
        return HexGrid.Instance.GetReachableCells(Owner.Location, Owner.ActionsAvailable);
    }

    private static List<HexCell> DiscoverReachableCells(Actor actor)
    {
        return actor.GetTrait<Mobile>().GetReachableCells();
    }

    private static void MoveToCell(Actor actor, List<HexCell> target)
    {
        actor.GetTrait<Mobile>().MoveToCell(target[0]);
    }

}