using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private readonly Dictionary<object, ActorAction> _actions = new Dictionary<object, ActorAction>();

    private readonly List<HexCell> _cellTargets = new List<HexCell>();

    private readonly List<object> _nonCellTargets = new List<object>();

    private Color _baseColor;

    private delegate void RevertDelegate();

    public string ActionId { get; set; }
    public object SelectedOption { get; set; }

    private Image Background => GetComponent<Image>();

    private string DisplayName
    {
        get
        {
            if (_nonCellTargets.Count == 1)
            {
                return ActionId + " (" + _nonCellTargets.First() + ")";
            }

            return ActionId;
        }
    }

    private Dropdown Dropdown => transform.Find("Dropdown").GetComponent<Dropdown>();
    private RevertDelegate Revert { get; set; }
    private ActorAction SelectedAction => _actions[SelectedOption];

    public void Execute()
    {
        Revert?.Invoke();

        SelectedAction.Invoke();
        SelectedAction.EntityContext.Mind.Act();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SystemController.Instance.ActiveAction != null && SystemController.Instance.ActiveAction.Revert != null)
        {
            SystemController.Instance.ActiveAction.Revert();
        }

        if (_nonCellTargets.Any())
        {
            SelectedOption = (Dropdown.options[Dropdown.value] as DataDropDown).Tag;

            Execute();
        }
        else
        {
            if (_cellTargets.Any())
            {
                foreach (var cell in _cellTargets)
                {
                    cell.EnableHighlight(Color.white);
                }

                Revert = () => { _cellTargets.ForEach(c => c.DisableHighlight()); };
            }

            SystemController.Instance.ActiveAction = this;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Background.color = Colors.Highlight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Background.color = _baseColor;
    }

    public void SetAction(ActorAction action)
    {
        ActionId = action.ActionName;

        _baseColor = Background.color;

        _actions.Add(action.Target, action);

        var cellTarget = action.Target as HexCell;
        if (cellTarget != null)
        {
            _cellTargets.Add(cellTarget);
        }
        else
        {
            _nonCellTargets.Add(action.Target);
            Dropdown.options.Add(new DataDropDown(action.Target.ToString(), action.Target));
        }

        if (_nonCellTargets.Count > 1)
        {
            Dropdown.gameObject.SetActive(true);
        }

        transform.Find("Text").GetComponent<Text>().text = DisplayName;
    }

    private class DataDropDown : Dropdown.OptionData
    {
        public readonly object Tag;

        public DataDropDown(string text, object tag) : base(text)
        {
            Tag = tag;
        }
    }
}