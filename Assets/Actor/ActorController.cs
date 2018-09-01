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


    private bool _init;

    [Range(1, 200)] public int InitialFactions = 50;

    public Faction PlayerFaction { get; set; }
    public Actor Player { get; set; }

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
        if (_init) return;

        _init = true;

        Player = GetPerson();
        Player.Traits.Remove(Player.GetTrait<Sentient>());
        Player.Traits.Add(new Player(Player));
        PlayerFaction = GetFaction(Player);
        PlayerFaction.name = "Player";

        for (var i = 0; i < InitialFactions; i++)
        {
            GetFaction(GetPerson());
        }
    }

    private Person GetPerson()
    {
        var personName = ActorHelper.GetRandomName();
        var personGameObject = new GameObject(personName);
        personGameObject.transform.parent = transform;

        var person = personGameObject.AddComponent(typeof(Person)) as Person;
        var sentient = new Sentient(person)
        {
            Physical = Random.Range(20, 80),
            Cunning = Random.Range(20, 80),
            Mental = Random.Range(20, 80),
            Charisma = Random.Range(20, 80)
        };

        person.AddTrait(sentient);
        person.Instantiate(personName);

        Actors.Add(person);

        return person;
    }

    private Faction GetFaction(Actor leader)
    {
        var factionName = leader.name + "'s Faction";

        var factionGameObject = new GameObject(factionName);
        factionGameObject.transform.parent = transform;

        var faction = factionGameObject.AddComponent(typeof(Faction)) as Faction;
        faction.SetLeader(leader);
        faction.Instantiate(factionName, TextureHelper.GetRandomColor());

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