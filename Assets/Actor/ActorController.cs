using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    private static ActorController _instance;

    private readonly List<Actor> _actors = new List<Actor>();
    private readonly List<Faction> _factions = new List<Faction>();

    public ActorPanel ActivePanel;

    public ActorDisplay ActorDisplayPrefab;

    public GameObject ActorPanelContainer;

    public ActorPanel ActorPanelPrefab;

    private bool init;

    public Faction PlayerFaction { get; set; }
    public Actor Player { get; set; }


    [Range(1, 200)] public int maxPersons = 50;
    [Range(1, 200)] public int personsPerFaction = 5;

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

    public List<Actor> Actors
    {
        get { return _actors; }
    }

    public List<Faction> Factions
    {
        get { return _factions; }
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
        Init();
    }

    public void Init()
    {
        if (!init)
        {
            init = true;

            Player = GetPerson();
            Player.Traits.Remove(Player.GetTrait<Sentient>());
            Player.Traits.Add(new Player(Player));
            PlayerFaction = GetFaction(Player);
            PlayerFaction.name = "Player";
            
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

        Factions.Add(faction);
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