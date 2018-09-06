﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

        if (Owner.Faction == ActorController.Instance.PlayerFaction)
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

    public override void DoPassive()
    {
    }

    public void MoveToCell(HexCell target)
    {
        if (Owner.Location != null)
        {
            var path = Pathfinder.FindPath(Owner.Location, target);
            Owner.ActionsAvailable -= (Pathfinder.GetPathCost(path) - Owner.Location.TravelCost);

            // move along path
            foreach (var cell in path)
            {
                Move(cell);
            }
        }
        else
        {
            // if owner is nowhere, move instantly
            Move(target);
        }
    }

    private void Move(HexCell target)
    {
        // moves instantly to location
        // use MoveToCell to move along a path
        Owner.Location = target;
        Owner.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);

        var sighted = Owner.GetTrait<Sighted>();

        if (sighted != null)
        {
            sighted.See();
        }
    }

    private List<HexCell> GetReachableCells()
    {
        return Pathfinder.GetReachableCells(Owner.Location, Owner.ActionsAvailable);
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