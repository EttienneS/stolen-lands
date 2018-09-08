using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDisplay : MonoBehaviour
{
    public delegate void RevertDelegate();

    public ActorAction Action;

    public RevertDelegate Revert { get; set; }


    public void Update()
    {
        transform.Find("Text").GetComponent<Text>().text = Action.ActionName;
    }

    public void Execute(HexCell cell)
    {
        Revert();

        Action.ActorContext.ActionPoints -= Action.ActAction(Action.ActorContext, cell);
    }

    public void OnClick()
    {
        if (SystemController.Instance.ActiveAction != null)
        {
            SystemController.Instance.ActiveAction.Revert();
        }

        var options = Action.DiscoverAction(Action.ActorContext);

        var cells = options as List<HexCell>;

        if (cells != null)
        {
            foreach (var cell in cells)
            {
                cell.EnableHighlight(Color.white);
            }

            Revert = () => { cells.ForEach(c => c.DisableHighlight()); };
        }
        else
        {
            var cell = options as HexCell;

            if (cell != null)
            {
                Revert = () => { cell.DisableHighlight(); };
            }
            else
            {
                
            }
        }

        SystemController.Instance.ActiveAction = this;
    }
}