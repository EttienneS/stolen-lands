using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public List<Faction> Factions { get; } = new List<Faction>();

    public ActorDisplay GetDisplayForActor(Actor actor)
    {
        var display = Instantiate(Instance.ActorDisplayPrefab);
        display.name = actor.name + " (Display)";
        display.GetComponent<ActorDisplay>().SetActor(actor);
        return display;
    }

    public void Init()
    {
        if (_init) return;

        _init = true;

        if (InitialFactions == 0)
        {
            return;
        }

        PlayerFaction = GetFaction(TextureHelper.GetRandomColor());
        PlayerFaction.AddMember(MakeBuilder(GetActor(new Player())));
        PlayerFaction.AddMember(MakeScout(GetActor(new Player())));

        for (var i = 0; i < InitialFactions; i++)
        {
            var faction = GetFaction(TextureHelper.GetRandomColor());

            faction.AddMember(MakeBuilder(GetActor(new AI())));
            faction.AddMember(MakeScout(GetActor(new AI())));
        }

        foreach (var faction in Factions)
        {
            faction.StartTurn();
        }
    }

    private Actor GetActor(Mind mind)
    {
        var actor = GetActorShell();
        actor.AddMind(mind);
        return actor;
    }

    public Actor MakeScout(Actor actor)
    {
        actor.AddTrait(new Sighted(4));
        actor.AddTrait(new Mobile(3, true));

        return actor;
    }

    public Actor MakeBuilder(Actor actor)
    {
        actor.AddTrait(new Sighted(1));
        actor.AddTrait(new Mobile(2, false));
        actor.AddTrait(new Builder());

        return actor;
    }

    private Actor GetActorShell()
    {
        var actor = Instantiate(ActorPrefab, transform);
        actor.name = ActorHelper.GetRandomName();
        return actor;
    }


    private Faction GetFaction(Color color, string factionName = "")
    {
        if (string.IsNullOrEmpty(factionName))
        {
            factionName = ActorHelper.GetRandomName();
        }

        var factionGameObject = new GameObject(factionName);
        factionGameObject.transform.parent = transform;

        var faction = factionGameObject.AddComponent(typeof(Faction)) as Faction;
        faction.Instantiate(factionName, color);

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
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Factions.Clear();

        var path = Path.Combine(location, "actor.data");
        using (var reader = File.OpenText(path))
        {
            var factionCount = reader.ReadLine().ToInt();
            for (var i = 0; i < factionCount; i++)
            {
                var factionName = reader.ReadLine();
                var color = GameHelpers.ColorFromString(reader.ReadLine());
                var faction = GetFaction(color, factionName);

                var actors = reader.ReadLine().ToInt();
                for (var a = 0; a < actors; a++)
                {
                    var actor = GetActorShell();
                    actor.name = reader.ReadLine();

                    HexGrid.Instance.Cells[reader.ReadLine().ToInt()].AddEntity(actor);

                    actor.ActionPoints = reader.ReadLine().ToInt();
                    actor.AddMind(MindFactory.Load(reader.ReadLine()));

                    var traits = reader.ReadLine().ToInt();
                    for (var t = 0; t < traits; t++)
                    {
                        actor.AddTrait(TraitFactory.Load(reader.ReadLine()));
                    }

                    faction.AddMember(actor);
                }
                var structs = reader.ReadLine().ToInt();

                for (var s = 0; s < structs; s++)
                {
                    StructureController.Instance.LoadBuilding(faction, reader.ReadLine(), HexGrid.Instance.Cells[reader.ReadLine().ToInt()]);
                }

                foreach (var cellId in reader.ReadLine().Split(',').Select(int.Parse))
                {
                    faction.KnownHexes.Add(HexGrid.Instance.Cells[cellId]);
                }

                foreach (var cellId in reader.ReadLine().Split(',').Select(int.Parse))
                {
                    faction.LearnHex(HexGrid.Instance.Cells[cellId]);
                }

                faction.Gold = reader.ReadLine().ToInt();
            }
        }

        PlayerFaction = Factions[0];
        PlayerFaction.StartTurn();
    }

    public void Save(string location)
    {
        var path = Path.Combine(location, "actor.data");

        using (var writer = File.CreateText(path))
        {
            writer.WriteLine(Factions.Count);
            foreach (var faction in Factions)
            {
                writer.WriteLine(faction.name);
                writer.WriteLine(GameHelpers.ColorToString(faction.Color));

                writer.WriteLine(faction.Members.Count);
                foreach (var actor in faction.Members)
                {
                    writer.WriteLine(actor.name);
                    writer.WriteLine(actor.Location.ID);
                    writer.WriteLine(actor.ActionPoints);

                    writer.WriteLine(actor.Mind.GetType().Name + ">" + actor.Mind.Save());

                    writer.WriteLine(actor.Traits.Count);
                    foreach (var trait in actor.Traits)
                    {
                        var traitString = trait.GetType().Name + ">" + trait.Save();
                        writer.WriteLine(traitString);
                    }
                }

                writer.WriteLine(faction.Holdings.Count);
                foreach (var holding in faction.Holdings)
                {
                    writer.WriteLine(holding.name);
                    writer.WriteLine(holding.Location.ID);
                }

                var knownCells = string.Empty;
                foreach (var cell in faction.KnownHexes)
                {
                    knownCells += cell.ID + ",";
                }
                writer.WriteLine(knownCells.Trim(','));

                var visibleCells = string.Empty;
                foreach (var cell in faction.VisibleHexes)
                {
                    visibleCells += cell.ID + ",";
                }
                writer.WriteLine(visibleCells.Trim(','));

                writer.WriteLine(faction.Gold);
            }
        }
    }
}
public class TraitFactory
{
    private static List<Type> _traitTypes;
    public static List<Type> AllTraitTypes
    {
        get
        {
            if (_traitTypes == null)
            {
                _traitTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                               from assemblyType in domainAssembly.GetTypes()
                               where typeof(Trait).IsAssignableFrom(assemblyType)
                               select assemblyType).ToList();
            }

            return _traitTypes;
        }
    }

    public static Trait Load(string readString)
    {
        var parts = readString.Split(new[] { '>' }, 2);

        var traitType = AllTraitTypes.First(t => t.Name == parts[0]);

        var trait = (Trait)Activator.CreateInstance(traitType);
        trait.Load(parts[1]);

        return trait;
    }
}

public class MindFactory
{
    private static List<Type> _mindTypes;
    public static List<Type> AllMindTypes
    {
        get
        {
            if (_mindTypes == null)
            {
                _mindTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                              from assemblyType in domainAssembly.GetTypes()
                              where typeof(Mind).IsAssignableFrom(assemblyType)
                              select assemblyType).ToList();
            }

            return _mindTypes;
        }
    }

    public static Mind Load(string readString)
    {
        var parts = readString.Split(new[] { '>' }, 2);

        var traitType = AllMindTypes.First(t => t.Name == parts[0]);

        var mind = (Mind)Activator.CreateInstance(traitType);
        mind.Load(parts[1]);

        return mind;
    }
}