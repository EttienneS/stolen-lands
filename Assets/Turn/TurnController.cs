using UnityEngine;

public class TurnController : MonoBehaviour
{
    private static TurnController _instance;

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

    private void Start()
    {
        foreach (var faction in ActorController.Instance.Factions)
        {
            var display = ActorController.Instance.GetDisplayForActor(faction.Leader);
            display.transform.SetParent(ScrollContentContainer.transform, false);
        }

        var playerLocation = ActorController.Instance.Player.Location.transform.position;
        CameraController.Instance.transform.position = new Vector3(playerLocation.x, playerLocation.y,
            CameraController.Instance.transform.position.z);
    }

    private void Update()
    {
    }

    public void EndCurrentTurn()
    {
        ActorController.Instance.Player.GetTrait<Player>().SpentActions = 0;

        foreach (var faction in ActorController.Instance.Factions)
        {
            if (faction == ActorController.Instance.PlayerFaction)
            {
                continue;
            }

            faction.TakeTurn();
        }

        ActorController.Instance.PlayerFaction.TakeTurn();
    }
}