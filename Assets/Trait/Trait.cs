using System.Collections.Generic;

public abstract class Trait
{
    public Entity Owner { get; set; }

    public string Name { get; set; }

    public abstract List<ActorAction> GetActions();

    public abstract void Start();

    public abstract void Finish();

}