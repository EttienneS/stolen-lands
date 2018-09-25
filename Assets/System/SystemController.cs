using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour
{
    private static SystemController _instance;
    
    public Actor SelectedActor { get; set; }
    
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

    public ActionDisplay ActiveAction { get; set; }

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
            for (var i = 1; i < 10; i++)
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

    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetSelectedActor(Actor actor)
    {
        ActiveAction?.Revert?.Invoke();

        if (SelectedActor != null)
        {
            SelectedActor.DisableOutline();
        }

        SelectedActor = actor;
        SelectedActor.EnableOutline();

        if (actor.Mind is Player)
        {
            actor.Mind.Act();
        }
    }

    public void Load()
    {
        var location = Application.persistentDataPath;

        HexGrid.Instance.Load(location);
        CameraController.Instance.Load(location);
        ActorController.Instance.Load(location);
        DoodadController.Instance.Load(location);
        StructureController.Instance.Load(location);
    }

    public void Save()
    {
        var location = Application.persistentDataPath;

        Directory.CreateDirectory(location);

        HexGrid.Instance.Save(location);
        CameraController.Instance.Save(location);
        ActorController.Instance.Save(location);
        DoodadController.Instance.Save(location);
        StructureController.Instance.Save(location);
    }
}