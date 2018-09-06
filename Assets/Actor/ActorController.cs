﻿using System.Collections.Generic;
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
        PlayerFaction.AddMember(GetActor());
        PlayerFaction.AddMember(GetActor());

        foreach (var actor in PlayerFaction.Members)
        {
            actor.Traits.Remove(actor.GetTrait<Sentient>());
            actor.Traits.Add(new Player(actor));
        }

        for (var i = 0; i < InitialFactions; i++)
        {
            var faction = GetFaction();
            faction.AddMember(GetActor());
            faction.AddMember(GetActor());

            faction.Members[0].AddTrait(new HexClaimer(faction.Members[0]));
        }
    }

    private Actor GetActor()
    {
        var actor = Instantiate(ActorPrefab, transform);
        actor.name = ActorHelper.GetRandomName();
        actor.AddTrait(new Sentient(actor)
        {
            Physical = Random.Range(20, 80),
            Cunning = Random.Range(20, 80),
            Mental = Random.Range(20, 80),
            Charisma = Random.Range(20, 80)
        });
        Actors.Add(actor);
        actor.AddTrait(new Sighted(actor, 2));
        actor.AddTrait(new Mobile(actor));
        actor.StartTurn();

        return actor;
    }

    private Faction GetFaction()
    {
        var factionName = ActorHelper.GetRandomName() + " Corp";

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