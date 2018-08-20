using System.Collections.Generic;
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

    private List<Actor> _actors = new List<Actor>();

    public List<Actor> Actors
    {
        get { return _actors; }
    }

    public ActorDisplay GetDisplayForActor(Actor actor)
    {
        var display = Instantiate(Instance.ActorDisplayPrefab);
        display.name = actor.name + " (Display)";
        display.GetComponent<ActorDisplay>().SetActor(actor);
        return display;
    }

    public void Awake()
    {
        var maxPersons = 100;
        var personsPerFaction = 1;
        var factions = maxPersons / personsPerFaction;

        for (int i = 0; i < factions; i++)
        {
            var leader = GetPerson();
            var faction = GetFaction(leader);

            for (int person = 0; person < personsPerFaction; person++)
            {
                faction.AddMember(GetPerson());
            }
        }
    }

    public Person GetPerson()
    {
        var name = ActorHelper.GetRandomName();
        var gameObject = new GameObject(name);
        gameObject.transform.parent = transform;

        var person = gameObject.AddComponent(typeof(Person)) as Person;
        var sentient = new Sentient(person)
        {
            Physical = Random.Range(20, 80),
            Cunning = Random.Range(20, 80),
            Mental = Random.Range(20, 80),
            Charisma = Random.Range(20, 80)
        };

        person.AddTrait(sentient);
        person.Instantiate(name, TextureHelper.GetRandomColor());

        Actors.Add(person);

        return person;
    } 

    public Faction GetFaction(Actor leader)
    {
        var name = leader.name + "'s Faction";

        var gameObject = new GameObject(name);
        gameObject.transform.parent = transform;

        var faction = gameObject.AddComponent(typeof(Faction)) as Faction;
        faction.SetLeader(leader);

        faction.AddTrait(new HexClaimer(faction));
        faction.Instantiate(name, TextureHelper.GetRandomColor());

        Actors.Add(faction); 

        return faction;
    }


    public void ShowActorPanel(Actor actor)
    {
        if (ActivePanel == null)
        {
            ActivePanel = Instantiate(ActorPanelPrefab, ActorPanelContainer.transform);
        }

        ActivePanel.name = actor.name + " (Info Panel)";
        ActivePanel.SetActor(actor);
    }
}