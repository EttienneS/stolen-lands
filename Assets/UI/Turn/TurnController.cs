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
        foreach (var member in ActorController.Instance.PlayerFaction.Members)
        {
            var display = ActorController.Instance.GetDisplayForActor(member);
            display.transform.SetParent(ScrollContentContainer.transform, false);
        }

        SystemController.Instance.SetSelectedActor(ActorController.Instance.PlayerFaction.Members[0]);

        var post = SystemController.Instance.SelectedActor.Location.transform.localPosition;
        CameraController.Instance.transform.position =
            new Vector3(post.x, post.y - 55, CameraController.Instance.transform.position.z);
    }

    public void EndCurrentTurn()
    {
        foreach (var faction in ActorController.Instance.Factions)
        {
            faction.ResetFog();
            faction.EndTurn();
            faction.StartTurn();
        }

        // re-select last selected actor
        SystemController.Instance.SetSelectedActor(SystemController.Instance.SelectedActor);
    }
}