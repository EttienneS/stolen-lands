using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour
{
    
    private static SystemController _instance;

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
    public Actor SelectedActor { get; set; }
    public void Continue()
    {
        //GameObject.Find("MenuCamera").GetComponent<PostProcessingBehaviour>().profile.colorGrading.enabled = true;

        SceneData.LoadMode = true;
        StartCoroutine(LoadNewScene("Main"));
    }

    public void Load()

    {
        // hex always first
        HexGrid.Instance.Load(SceneData.SaveFolder);

        // doodads depend on world so after hex
        DoodadController.Instance.Load(SceneData.SaveFolder);

        // actor depend on world so after hex
        ActorController.Instance.Load(SceneData.SaveFolder);

        // camera can load at any time so load last
        CameraController.Instance.Load(SceneData.SaveFolder);
    }

    public void LoadMain()
    {
        StartCoroutine(LoadNewScene("Main"));
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadNewScene("MainMenu"));
    }

    public void Save()
    {
        Directory.CreateDirectory(SceneData.SaveFolder);

        // hex always first
        HexGrid.Instance.Save(SceneData.SaveFolder);

        // doodads depend on world so after hex
        DoodadController.Instance.Save(SceneData.SaveFolder);

        // actor depend on world so after hex
        ActorController.Instance.Save(SceneData.SaveFolder);

        // camera can save at any time so save last
        CameraController.Instance.Save(SceneData.SaveFolder);
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

    public void Awake()
    {
        if (SceneData.LoadMode)
        {
            SceneData.LoadMode = false;
            Load();
        }
        else
        {
            ActorController.Instance.Init();
            HexGrid.Instance.Init();
            TurnController.Instance.Init();
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

    public static IEnumerator LoadNewScene(string screen)
    {
        var async = SceneManager.LoadSceneAsync(screen);
        while (!async.isDone)
        {
            yield return null;
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
}
