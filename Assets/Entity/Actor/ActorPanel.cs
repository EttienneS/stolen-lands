using UnityEngine;
using UnityEngine.UI;

public class ActorPanel : MonoBehaviour
{
    public Actor Actor;

    private void Start()
    {
        if (Actor != null)
        {
            SetActor(Actor);
        }
    }

    public void SetActor(Actor actor)
    {
        Actor = actor;

        transform.Find("Icon").GetComponent<Image>().sprite = Actor.Sprite;
        transform.Find("Name").GetComponent<Text>().text = Actor.name;

        var stats = transform.Find("Stats").transform;

        if (actor.Mind != null)
        {
            stats.Find("Physical").GetComponent<Text>().text = actor.Mind.Physical.ToString();
            stats.Find("Cunning").GetComponent<Text>().text = actor.Mind.Cunning.ToString();
            stats.Find("Mental").GetComponent<Text>().text = actor.Mind.Mental.ToString();
            stats.Find("Charisma").GetComponent<Text>().text = actor.Mind.Charisma.ToString();
        }

        stats.Find("Location").GetComponent<Text>().text = Actor.Location.ToString();
    }

    private void Update()
    {
    }
}