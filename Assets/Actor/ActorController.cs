using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    private static ActorController _instance;

    private readonly List<Actor> _actors = new List<Actor>();
    private readonly List<Faction> _factions = new List<Faction>();

    private bool _init;

    private ActorPanel ActivePanel;

    public ActorDisplay ActorDisplayPrefab;

    public GameObject ActorPanelContainer;

    public ActorPanel ActorPanelPrefab;


    public Actor ActorPrefab;

    [Range(1, 200)] public int InitialFactions = 50;

    public Faction PlayerFaction { get; set; }

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


        PlayerFaction = GetFaction();
        PlayerFaction.AddMember(GetActor(new Player()));
        PlayerFaction.AddMember(GetActor(new Player()));


        for (var i = 0; i < InitialFactions; i++)
        {
            var faction = GetFaction();
            faction.AddMember(GetActor(new Sentient()));
            faction.AddMember(GetActor(new Sentient()));

            faction.Members[0].AddTrait(new HexClaimer());
        }
    }

    private Actor GetActor(Trait brain)
    {
        var actor = Instantiate(ActorPrefab, transform);
        actor.name = ActorHelper.GetRandomName();
        actor.AddTrait(brain);
        Actors.Add(actor);
        actor.AddTrait(new Sighted(3));
        actor.AddTrait(new Mobile(3));
        actor.AddTrait(new Builder());
        actor.StartTurn();

        return actor;
    }

    private Faction GetFaction()
    {
        var factionName = ActorHelper.GetRandomName();

        var factionGameObject = new GameObject(factionName);
        factionGameObject.transform.parent = transform;

        var faction = factionGameObject.AddComponent(typeof(Faction)) as Faction;
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