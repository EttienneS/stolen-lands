using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public static TurnController _instance;
    private readonly Color ActiveColor = new Color(0.7f, 0.02f, 0.02f, 0.5f);
    private readonly Color InactiveColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);

    public GameObject ScrollContentContainer;

    private bool TakeTurns;

    public static TurnController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("TurnController").GetComponent<TurnController>();
            }

            return _instance;
        }
    }

    public int ActiveActorIndex { get; set; }

    public Actor ActiveActor
    {
        get { return ActorController.Instance.Actors[ActiveActorIndex]; }
    }

    private void Start()
    {

        foreach (var actor in ActorController.Instance.Actors)
        {
            var display = ActorController.Instance.GetDisplayForActor(actor);
            display.transform.SetParent(ScrollContentContainer.transform, false);
        }

        ActiveActorIndex = 0;
        // ActorController.Instance.ShowActorPanel(ActorController.Instance.Actors[ActiveActorIndex]);

        // uncomment the next line to spam the endturn button 100 times per second
        // InvokeRepeating("EndCurrentTurn", 0, 0.01f);
    }

    private void Update()
    {
        foreach (var actorDisplay in GetComponentsInChildren<ActorDisplay>())
        {
            actorDisplay.GetComponent<Image>().color =
                ActorController.Instance.Actors[ActiveActorIndex] == actorDisplay.Actor
                    ? ActiveColor
                    : InactiveColor;
        }

        TakeTurn();
    }

    public void EndCurrentTurn()
    {
        TakeTurns = !TakeTurns;
    }

    public void TakeTurn()
    {
        if (TakeTurns)
        {
            // each actor take 1 turn each frame
            for (int i = 0; i < ActorController.Instance.Actors.Count; i++)
            {
                var activeActor = ActorController.Instance.Actors[ActiveActorIndex];
                activeActor.TakeTurn();

                ActiveActorIndex++;

                if (ActiveActorIndex >= ActorController.Instance.Actors.Count)
                {
                    ActiveActorIndex = 0;
                }

                // ActorController.Instance.ShowActorPanel(ActorController.Instance.Actors[ActiveActorIndex]);
            }

        }
    }
}