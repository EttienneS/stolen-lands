using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDisplay : MonoBehaviour
{
    public ActorController ActorController;
    public GameObject PlayerPrefab;

    void Start()
    {
        // offset from the middle of the panel
        var height = Mathf.FloorToInt(PlayerPrefab.GetComponent<RectTransform>().rect.height + 10);

        // start point, 0 is the middle of the panel so we go up and then down from there
        var top = (transform.GetComponent<RectTransform>().rect.height / 2) - ((height / 2) + 5);

        foreach (var actor in ActorController.Actors)
        {
            var display = Instantiate(PlayerPrefab);
            display.transform.SetParent(transform, false);

            display.GetComponent<ActorDisplay>().SetActor(actor);
            display.transform.localPosition = new Vector2(0, top);

            // move the top down by the height + 10;
            top -= height;

        }
    }

    void Update()
    {
    }
}
