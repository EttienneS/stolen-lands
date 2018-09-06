using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDisplay : MonoBehaviour
{
    public delegate void ActionCompletedDelegate(ActorAction executedAction, int cost);

    public SpriteRenderer ActionIndicatorPrefab;

    public List<ActorAction> Actions = new List<ActorAction>();

    public HexCell Context { get; set; }

    public ActionCompletedDelegate ActionCompleted { get; set; }

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

        ActionCompleted(executedAction, executedAction.ActAction(executedAction.ActorContext, Context));
    }

    public void Start()
    {
        if (Actions.Count == 1)
        {
            var action = Actions[0];
            var cost = action.GetCost(action.ActorContext, Context);
            var spacing = 6;
            var offset = cost * spacing / 2;
            for (var i = 0; i < cost; i++)
            {
                var indicator = Instantiate(ActionIndicatorPrefab, transform);
                indicator.transform.localPosition =
                    new Vector3(i * spacing - offset, -7, indicator.transform.localPosition.z);
                indicator.transform.localScale = new Vector3(4, 4);
            }
        }
    }
}