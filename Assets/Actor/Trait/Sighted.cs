using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Sighted : Trait
{
    public Sighted(Actor owner, int visionRange) : base(owner)
    {
        _visionRange = visionRange;
    }

    private int _visionRange;

    public override List<ActorAction> GetActions()
    {
        var actions = new List<ActorAction>();
        return actions;
    }

    public override void DoPassive()
    {
        See();
    }

    public void See()
    {
        if (Owner.Location == null)
        {
            return;
        }

        var center = Owner.Location;
        
        var visible = new List<HexCell>{ center };
        var frontier = center.neighbors.ToList();

        var done = new List<HexCell> { center };

        while (frontier.Any())
        {
            var cell = frontier.First();
            frontier.RemoveAt(0);

            if (cell != null && !done.Contains(cell))
            {
                if (cell.coordinates.DistanceTo(Owner.Location.coordinates) <= _visionRange)
                {
                    visible.Add(cell);
                    frontier.AddRange(cell.neighbors);
                }
            }

            done.Add(cell);
        }

        foreach (var hex in visible)
        {
            Owner.Faction.LearnHex(hex);
        }

    }
}