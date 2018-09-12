using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ActorAction Action;

    public delegate void RevertDelegate();
    public List<HexCell> CellTargets { get; set; }
    public Dropdown Dropdown
    {
        get { return transform.Find("Dropdown").GetComponent<Dropdown>(); }
    }

    public Dictionary<string, object> Options { get; set; }
    public RevertDelegate Revert { get; set; }

    public void Execute(object option)
    {
        if (Revert != null)
        {
            Revert();
        }

        Action.EntityContext.ActionPoints -= Action.ActAction(Action.EntityContext, option);

        (Action.EntityContext as Actor).TakeTurn();
    }

    public void SetAction(ActorAction action)
    {
        _baseColor = Background.color;

        Action = action;

        var options = Action.DiscoverAction(Action.EntityContext);

        transform.Find("Text").GetComponent<Text>().text = Action.ActionName;

        CellTargets = new List<HexCell>();
        Options = new Dictionary<string, object>();

        var cells = options as List<HexCell>;

        if (cells != null)
        {
            CellTargets.AddRange(cells);
        }
        else
        {
            var cell = options as HexCell;
            if (cell != null)
            {
                CellTargets.Add(cell);
            }
            else
            {
                Dropdown.gameObject.SetActive(true);

                foreach (var option in options as List<object>)
                {
                    Options.Add(option.ToString(), option);
                    Dropdown.options.Add(new Dropdown.OptionData(Options.Last().Key));
                }
            }
        }
    }

    private Color _baseColor;

    private Image Background
    {
        get { return GetComponent<Image>(); }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Background.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Background.color = _baseColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SystemController.Instance.ActiveAction != null  && SystemController.Instance.ActiveAction.Revert != null)
        {
            SystemController.Instance.ActiveAction.Revert();
        }

        if (Options.Any())
        {
            Execute(Options[Dropdown.options[Dropdown.value].text]);
        }
        else
        {
            if (CellTargets.Any())
            {
                foreach (var cell in CellTargets)
                {
                    cell.EnableHighlight(Color.white);
                }

                Revert = () => { CellTargets.ForEach(c => c.DisableHighlight()); };
            }

            SystemController.Instance.ActiveAction = this;
        }
    }
}