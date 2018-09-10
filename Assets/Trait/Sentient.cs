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

    public Sentient()
    {
        Physical = Random.Range(20, 80);
        Cunning = Random.Range(20, 80);
        Mental = Random.Range(20, 80);
        Charisma = Random.Range(20, 80);
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

            foreach (var action in (Owner as Actor).AvailableActions)
            {
                var cells = action.DiscoverAction(Owner) as List<HexCell>;

                if (cells != null)
                {
                    foreach (var context in cells)
                    {
                        if (action.CanExecute((Owner as Actor), context))
                        {
                            if (!afforadableActionContexts.ContainsKey(action.ActAction))
                            {
                                afforadableActionContexts.Add(action.ActAction, new List<HexCell>());
                            }

                            afforadableActionContexts[action.ActAction].Add(context);
                        }
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

            Owner.ActionPoints -= randomAction.Invoke(Owner, targets[Random.Range(0, targets.Count - 1)]);
        }
    }
}