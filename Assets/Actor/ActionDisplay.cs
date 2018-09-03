using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDisplay : MonoBehaviour
{
    public delegate void RefreshPlayerActionDelegate(ActorAction executedAction);

    public List<ActorAction> Actions = new List<ActorAction>();

    public HexCell Context { get; set; }

    public RefreshPlayerActionDelegate RefreshPlayerAction { get; set; }

    public void Update()
    {
        if (Actions.Count == 1)
        {
            transform.Find("Text").GetComponent<Text>().text = Actions[0].ActionName;
        }
        else
        {
            transform.Find("Text").GetComponent<Text>().text = "...";
        }
    }

    public void OnClick()
    {
        var executedAction = Actions[0];
        executedAction.ActAction(executedAction.ActorContext, new List<HexCell> {Context});

        if (RefreshPlayerAction != null)
        {
            RefreshPlayerAction(executedAction);
        }
    }
}