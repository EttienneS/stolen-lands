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

        stats.Find("Physical").GetComponent<Text>().text = Actor.Physical.ToString();
        stats.Find("Cunning").GetComponent<Text>().text = Actor.Cunning.ToString();
        stats.Find("Mental").GetComponent<Text>().text = Actor.Mental.ToString();
        stats.Find("Charisma").GetComponent<Text>().text = Actor.Charisma.ToString();
    }

    private void Update()
    {
    }
}