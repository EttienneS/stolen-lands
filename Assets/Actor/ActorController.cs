using UnityEngine;

public class ActorController : MonoBehaviour
{
    private static ActorController _actorControllerInstance;

    public GameObject ActorPanelContainer;

    public ActorPanel ActorPanelPrefab;

    public static ActorController ActorControllerInstance
    {
        get
        {
            if (_actorControllerInstance == null)
            {
                _actorControllerInstance = GameObject.Find("ActorController").GetComponent<ActorController>();
            }

            return _actorControllerInstance;
        }
    }


    public Actor[] Actors
    {
        get { return GetComponentsInChildren<Actor>(); }
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
        var panel = Instantiate(ActorPanelPrefab, ActorPanelContainer.transform);
        panel.name = actor.Name + " (Info Panel)";
        panel.SetActor(actor);
    }
}