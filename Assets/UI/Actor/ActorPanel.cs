using UnityEngine;
using UnityEngine.UI;

public class ActorPanel : MonoBehaviour
{
    public Actor Actor;

    private Image Icon;
    private Text Name;

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

        // on init set the values to match the given Actor
        Icon = transform.Find("Icon").GetComponent<Image>();
        Name = transform.Find("Name").GetComponent<Text>();

        Icon.sprite = Actor.Sprite;
        Name.text = Actor.Name;
    }

    private void Update()
    {
    }
}