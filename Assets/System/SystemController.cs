using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour
{
    private static SystemController _instance;

    public Canvas GridCanvas;

    public Canvas UICanvas;

    public static SystemController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("System").GetComponent<SystemController>();
            }

            return _instance;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TurnController.Instance.EndCurrentTurn();
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            var index = 0;
            if (SelectedActor != null)
            {
                index = ActorController.Instance.PlayerFaction.Members.IndexOf(SelectedActor);
            }

            index++;

            if (index > ActorController.Instance.PlayerFaction.Members.Count - 1)
            {
                index = 0;
            }

            SetSelectedActor(ActorController.Instance.PlayerFaction.Members[index]);

            CameraController.Instance.MoveToViewCell(SelectedActor.Location);

        }
        else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
        else
        {
            for (int i = 1; i < 10; i++)
            {
                if (Input.GetKeyUp("" + i))
                {
                    ControlPanelController.Instance.InvokeAction(i);
                    break;
                }
            }
        }
    }

    private void HandleInput()
    {
        var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            if (ActiveAction != null)
            {
                var selectedCell = HexGrid.Instance.GetCellAtPoint(hit.point);

                if (selectedCell.Highlight.enabled)
                {
                    ActiveAction.SelectedOption = selectedCell;
                    ActiveAction.Execute();
                }
            }
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("Loading");
    }

    public Actor SelectedActor;

    public ActionDisplay ActiveAction { get; set; }
    public void SetSelectedActor(Actor actor)
    {
        if (SelectedActor != null)
        {
            SelectedActor.DisableOutline();
        }

        SelectedActor = actor;
        SelectedActor.EnableOutline();

        var player = actor.Mind is Player;
        if (player != null)
        {
            actor.Mind.Act();
        }
    }
}