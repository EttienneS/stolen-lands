using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    private static ActorController _instance;

    private bool _init;

    private ActorPanel ActivePanel;

    public ActorDisplay ActorDisplayPrefab;

    public GameObject ActorPanelContainer;

    public ActorPanel ActorPanelPrefab;

    public Actor ActorPrefab;

    [Range(0, 200)] public int InitialFactions = 50;

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

    public List<Actor> Actors { get; } = new List<Actor>();

    public List<Faction> Factions { get; } = new List<Faction>();

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

        if (InitialFactions == 0)
        {
            return;
        }

        PlayerFaction = GetFaction();
        PlayerFaction.AddMember(GetActor(new Player()));

        for (var i = 0; i < InitialFactions; i++)
        {
            var faction = GetFaction();
            faction.AddMember(GetActor(new AI()));
        }
    }

    private Actor GetActor(Mind mind)
    {
        var actor = Instantiate(ActorPrefab, transform);
        actor.name = ActorHelper.GetRandomName();

        actor.Mind = mind;
        mind.Entity = actor;

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

    public void Load(string location)
    {
    }

    public void Save(string location)
    {
    }
}