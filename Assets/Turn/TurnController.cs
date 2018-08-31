using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public static TurnController _instance;

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
            var display = ActorController.Instance.GetDisplayForActor(faction);
            display.transform.SetParent(ScrollContentContainer.transform, false);
        }

        var playerLocation = ActorController.Instance.PlayerFaction.Location.transform.position;
        CameraController.Instance.transform.position = new Vector3(playerLocation.x, playerLocation.y, CameraController.Instance.transform.position.z);
    }

    private void Update()
    {
       
    }

    public void EndCurrentTurn()
    {
        ActorController.Instance.Player.GetTrait<Player>().SpentActions = 0;


        foreach (var faction in ActorController.Instance.Factions)
        {
            faction.Leader.TakeTurn();
        }
    }

}