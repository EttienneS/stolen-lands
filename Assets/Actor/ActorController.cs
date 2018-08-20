using System.Linq;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    private static ActorController _instance;

    public ActorPanel ActivePanel;

    public ActorDisplay ActorDisplayPrefab;

    public GameObject ActorPanelContainer;

    public ActorPanel ActorPanelPrefab;

    public static ActorController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("ActorController").GetComponent<ActorController>();
            }

            return _instance;
        }
    }

    public Actor[] Actors
    {
        get { return GetComponentsInChildren<Actor>(); }
    }

    public ActorDisplay GetDisplayForActor(Actor actor)
    {
        var display = Instantiate(Instance.ActorDisplayPrefab);
        display.name = actor.Name + " (Display)";
        display.GetComponent<ActorDisplay>().SetActor(actor);
        return display;
    }

    public void Awake()
    {
        var maxPersons = 100;
        var personsPerFaction = 5;
        var factions = maxPersons / personsPerFaction;

        for (int i = 0; i < maxPersons; i++)
        {
            Person.GetPerson(transform);
        }

        var persons = Actors.ToList();

        for (int i = 0; i < factions; i++)
        {
            var faction = Faction.GetFaction(transform, persons.First());

            for (int p = 0; p < personsPerFaction; p++)
            {
                var person = persons.First();
                person.AddTrait(new FactionMember(person, faction));
                faction.Members.Add(person.GetTrait<FactionMember>());
                persons.RemoveAt(0);
            }
        }
    }

    public void ShowActorPanel(Actor actor)
    {
        if (ActivePanel == null)
        {
            ActivePanel = Instantiate(ActorPanelPrefab, ActorPanelContainer.transform);
        }

        ActivePanel.name = actor.Name + " (Info Panel)";
        ActivePanel.SetActor(actor);
    }
}