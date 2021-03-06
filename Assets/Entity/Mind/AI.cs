using System.Linq;
using UnityEngine;

public class AI : Mind
{
    public override void Act()
    {
        while (true)
        {
            var availableActions = Entity.AvailableActions;
            if (!availableActions.Any())
            {
                return;
            }

            var groupedActions = availableActions.GroupBy(a => a.ActionName)
                .ToDictionary(g => g.Key, v => v.ToList());

            var keys = groupedActions.Keys.ToList();
            var randomAction = keys[Random.Range(0, keys.Count)];

            var actions = groupedActions[randomAction];

            actions[Random.Range(0, actions.Count)].Invoke();
        }
    }

    public override string Save()
    {
        return string.Empty;
    }

    public override void Load(string data)
    {
    }
}