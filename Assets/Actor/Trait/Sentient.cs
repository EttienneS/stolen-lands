using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public override void DoPassive()
    {
    }

    public virtual void TakeAction(List<ActorAction> allActions)
    {
        var afforadableActionContexts = new Dictionary<ActorAction.Act, List<HexCell>>();


        while (true)
        {
            afforadableActionContexts.Clear();

            foreach (var action in Owner.AvailableActions)
            {
                foreach (var context in action.DiscoverAction(Owner))
                {
                    if (action.CanExecute(Owner, context))
                    {
                        if (!afforadableActionContexts.ContainsKey(action.ActAction))
                        {
                            afforadableActionContexts.Add(action.ActAction, new List<HexCell>());
                        }

                        afforadableActionContexts[action.ActAction].Add(context);
                    }
                }
            }

            if (!afforadableActionContexts.Any())
            {
                // no actions available that has an effective cost we can affort
                break;
            }

            var randomAction =
                afforadableActionContexts.Keys.ToList()[Random.Range(0, afforadableActionContexts.Keys.Count - 1)];
            var targets = afforadableActionContexts[randomAction];

            Owner.ActionsAvailable -= randomAction.Invoke(Owner, targets[Random.Range(0, targets.Count - 1)]);
        }
    }
}