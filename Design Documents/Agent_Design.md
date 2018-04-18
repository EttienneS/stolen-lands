# Agent

## Overview:

At the core of the game is the agent, everything in the game is an agent with varying levels of potential influence on the world.

Everything from a lowly goblin to the player of the game should be based on the base concept of an agent.

## Example of the agent base type:

```csharp
interface IAgent : IEntity
{
    // invoked when a turn completes to allow the agent to take a turn
    void TakeTurn();

     // an action every agent should have, the ability to share its known context with another.
    void ShareContext(IAgent confidant);

     // an action every agent should have, the ability to gain context from a source, this is a seperate method as the agent has the option to interperet the context
    void GainContext(IContext context, IAgent source);
}
```

* **Name**: Each agent should have some unique way of being identified, if nothing else this is always at least one bit of context it knows

* **MyContext**: A core tenant of the game is that information is not reliable there exists a 'true' WorldContext but nothing should have access to this.  An agent should make its descisions based on this.  Worl

* **TakeTurn**: This is what the agent wants to do based on its needs and known context.

* **ShareContext**: Each agent always knows something, even it is only its own internal properties and can share this context with other agents (by choice or by force).

## Example Agent:

```csharp
public class Goblin: IAgent
{
    string ID {get; set;}

    Location MyLocation {get; set;}
     
    IContext MyContext {get; set;}

    // a goblin is arbitrarily worth 10
    // this is important when agents has control over lesser agents
    int Worth {get; set;} => 10; 

    void TakeTurn()
    {
        Settlement closestSettlement;
        foreach (var settlement in MyContext.Entities.OfType<Settlement>())
        {
            // find closest settlment
        }

        // does not know of any settlements
        if (closestSettlement != null)
        {
            // search for settlement
        }
        else
        {
            // i am at the settlement, raid it for whatever!
            if (MyLocation == closestSettlement.Location)
            {
                closestSettlement.Raid(this);
            }
            else
            {
                // tell the worldstate that I want to move closer to the settlement
                // worldstate then decides how that happens based on my properties
                // *do not do individual pathing for each entity*
                World.MoveTo(this, closestSettlement.Location);
            }
        }
    }

    void ShareContext(IAgent confidant)
    {
        if (confidant is Goblin)
        {
            // tell my goblin pal of all the settlements I know of
            confidant.GainContext(MyContext.Entities.OfType<Settlement>());
        }
        else
        {
            // I dont trust this entity, only reveal my ID
            confidant.GainContext(ID);
        }
    }

    void GainContext(IContext context, IAgent source)
    {
        if (source is Goblin)
        {
            // my fellow goblins never lie to me, believe him
            MyContext.Merge(context);
            MyContext.Entities.Find(source.ID).Tags.Add("Close personal friend");
            
        }
        else
        {
            // this source is nonsense, he must be bad
            MyContext.Entities.Find(source.ID).Tags.Add("Enemy");
        }
    }

}
```