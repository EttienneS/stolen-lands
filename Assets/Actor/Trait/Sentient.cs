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

    public void TakeAction(List<ActorAction> allActions)
    {
        // 100 mental, 100 cunning == 10 actions
        // 100 mental, 10 cunning == 1 actions

        var actionDivisor = (110 - Cunning) / 10;
        var actionsAvailable = Mental / actionDivisor;

        while (actionsAvailable > 0)
        {
            var availableActions = allActions.Where(a => a.Cost <= actionsAvailable).ToArray();

            if (availableActions.Length == 0)
            {
                // no actions available that has an effective cost we can affort
                break;
            }

            var randomAction = availableActions[Random.Range(0, availableActions.Length - 1)];
            actionsAvailable -= randomAction.Cost;
            randomAction.Execute();
        }
    }
}