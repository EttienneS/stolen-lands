using UnityEngine;

public class ActorController : MonoBehaviour
{
    private static ActorController _instance;

    public GameObject ActorPanelContainer;

    public ActorPanel ActorPanelPrefab;

    public ActorDisplay ActorDisplayPrefab;

    public ActorPanel ActivePanel;
    

    public static ActorController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("ActorController").GetComponent<ActorController>();
            }

            return _instance;
        }
    }


    public Actor[] Actors
    {
        get { return GetComponentsInChildren<Actor>(); }
    }

    public ActorDisplay GetDisplayForActor(Actor actor)
    {
        var display = Instantiate(ActorController.Instance.ActorDisplayPrefab);
        display.name = actor.Name + " (Display)";
        display.GetComponent<ActorDisplay>().SetActor(actor);
        return display;
    }

    public void Awake()
    {
        for (int i = 0; i < 50; i++)
        {
            Person.GetAveragePerson(transform);            
        }

       
    }

    public void ShowActorPanel(Actor actor)
    {
        if (ActivePanel == null)
        {
            ActivePanel = Instantiate(ActorPanelPrefab, ActorPanelContainer.transform);
        }

        ActivePanel.name = actor.Name + " (Info Panel)";
        ActivePanel.SetActor(actor);
    }
}