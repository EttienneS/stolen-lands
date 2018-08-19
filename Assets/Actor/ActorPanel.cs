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
        transform.Find("Name").GetComponent<Text>().text = Actor.Name;

        var stats = transform.Find("Stats").transform;

        var sentience = actor.GetTrait<Sentient>();

        if (sentience != null)
        {

            stats.Find("Physical").GetComponent<Text>().text = sentience.Physical.ToString();
            stats.Find("Cunning").GetComponent<Text>().text = sentience.Cunning.ToString();
            stats.Find("Mental").GetComponent<Text>().text = sentience.Mental.ToString();
            stats.Find("Charisma").GetComponent<Text>().text = sentience.Charisma.ToString();
        }

        stats.Find("Location").GetComponent<Text>().text = Actor.Location.ToString();
    }

    private void Update()
    {
    }
}