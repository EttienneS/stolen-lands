using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    private readonly Color ActiveColor = new Color(0.7f, 0.02f, 0.02f, 0.5f);
    public ActorController ActorController;
    private readonly Color InactiveColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    public GameObject PlayerPrefab;

    public int ActiveActorIndex { get; set; }

    public Actor ActiveActor
    {
        get { return ActorController.Actors[ActiveActorIndex]; }
    }

    private void Start()
    {
        // offset from the middle of the panel
        var height = Mathf.FloorToInt(PlayerPrefab.GetComponent<RectTransform>().rect.height + 10);

        // start point, 0 is the middle of the panel so we go up and then down from there
        var top = transform.GetComponent<RectTransform>().rect.height / 2 - (height / 2 + 5);

        foreach (var actor in ActorController.Actors)
        {
            var display = Instantiate(PlayerPrefab);
            display.name = actor.Name + " (Turn Display)";
            display.transform.SetParent(transform, false);

            display.GetComponent<ActorDisplay>().SetActor(actor);
            display.transform.localPosition = new Vector2(0, top);

            // move the top down by the height + 10;
            top -= height;
        }

        ActiveActorIndex = 0;
    }

    private void Update()
    {
        foreach (var actorDisplay in GetComponentsInChildren<ActorDisplay>())
        {
            actorDisplay.GetComponent<Image>().color = ActorController.Actors[ActiveActorIndex] == actorDisplay.Actor
                ? ActiveColor
                : InactiveColor;
        }
    }

    public void EndCurrentTurn()
    {
        ActiveActorIndex++;

        if (ActiveActorIndex >= ActorController.Actors.Length)
        {
            ActiveActorIndex = 0;
        }
    }
}