using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlPanelController : MonoBehaviour
{
    private static ControlPanelController _instance;

    private readonly List<ActionDisplay> _playerActions = new List<ActionDisplay>();
    public ActionDisplay ActionPrefab;

    public static ControlPanelController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("ControlPanel").GetComponent<ControlPanelController>();
            }

            return _instance;
        }
    }

    public void AddAction(ActorAction action)
    {
        var group = _playerActions.FirstOrDefault(p => p.ActionId == action.ActionName);

        if (group == null)
        {
            group = Instantiate(ActionPrefab, transform);
            _playerActions.Add(group);
        }

        group.SetAction(action);
    }

    public void InvokeAction(int number)
    {
        // from 1 index to 0 index
        number--;
        if (_playerActions.Count - 1 >= number)
        {
            _playerActions[number].OnPointerClick(null);
        }
    }

    public void ClearPlayerActions()
    {
        foreach (var action in _playerActions)
        {
            Destroy(action.gameObject, 0);
        }

        _playerActions.Clear();
    }
}