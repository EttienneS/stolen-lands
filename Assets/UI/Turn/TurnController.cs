using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    private readonly Color ActiveColor = new Color(0.7f, 0.02f, 0.02f, 0.5f);
    private readonly Color InactiveColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    public GameObject PlayerPrefab;

    public int ActiveActorIndex { get; set; }

    public Actor ActiveActor
    {
        get { return ActorController.ActorControllerInstance.Actors[ActiveActorIndex]; }
    }

    private void Start()
    {
        foreach (var actor in ActorController.ActorControllerInstance.Actors)
        {
            var display = Instantiate(PlayerPrefab);
            display.name = actor.Name + " (Turn Display)";
            display.transform.SetParent(transform, false);
            display.GetComponent<ActorDisplay>().SetActor(actor);
        }

        ActiveActorIndex = 0;
        ActorController.ActorControllerInstance.ShowActorPanel(ActorController.ActorControllerInstance.Actors[ActiveActorIndex]);
    }

    private void Update()
    {
        foreach (var actorDisplay in GetComponentsInChildren<ActorDisplay>())
        {
            actorDisplay.GetComponent<Image>().color = ActorController.ActorControllerInstance.Actors[ActiveActorIndex] == actorDisplay.Actor
                ? ActiveColor
                : InactiveColor;
        }
    }

    public void EndCurrentTurn()
    {
        ActiveActorIndex++;

        if (ActiveActorIndex >= ActorController.ActorControllerInstance.Actors.Length)
        {
            ActiveActorIndex = 0;
        }

        ActorController.ActorControllerInstance.ShowActorPanel(ActorController.ActorControllerInstance.Actors[ActiveActorIndex]);
    }
}