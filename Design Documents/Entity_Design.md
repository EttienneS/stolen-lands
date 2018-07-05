# Entity

## Overview:

Every object in the game is an entity at the lowest level sharing certain basic properties.

## Example of the agent base type:

```csharp
interface IEntity
{
    // some way of identifying itself
    string ID {get; set;}
    
    // everything has to be somewhere
    Location MyLocation {get; set;}

    // everything has an intrinsic value, this can be seen as an abstracted 
    // 'importance score' to be used as a way to figure out priorities
    int Worth {get; set;}

    // a list of tags that applies to this entity, this can be used to 
    // extend the entity to have properties greater than what can be hard 
    // into it
    List<ITag> Tags {get; set;}
}

interface ITag
{
    // a tag changes the associated entity in some way
    Modify(IEntity entity);
}

```

## Implementation of an enitity and tag:

```csharp
public Rock : IEntity
{
    string ID {get; set;} 
    
    Location MyLocation {get; set;}

    // random rocks are worthless
    int Worth {get; set;} => 0;

    // rocks are hard, but other things may be harder, so create a Hard tag with a property, when we later have something try to destroy the rock we can ask it how Hard it is
    List<ITag> Tags {get; set; } => new List<ITag>(new Hard(5));    
}

public Hard : ITag
{
    int Hardness {get;set;}

    Modify(IEntity entity)
    {
        // do nothing, we do not need to change anything on the entity, this tag is a container for information
    }

    bool CanBreak(int forceOfImpact)
    {
        // something hit me, am I broken?
        if (forceofImpact > Harness)
        {
            // busted..
            return true;
        }
        else
        {
            // not broken but weakened a bit, another hit may break me..
            Harness--;
            return false;
        }
    }
}

```

## Example of entity interaction

```csharp

var randoGoblin = World.Entities.OfType<Goblin>().First();
var newRock = new Rock();

if (randoGoblin.Attack(newRock))
{
    Message("The goblin smashes the rock and cackles with glee");
}
else
{
    Message("The goblin fails to smash the rock and hurts itself.");
    randoGoblin.Health--;
}

```

## Example of how the attack would be handles by the worldstate

```csharp

public Goblin : CreatureBase
{
    // properties of the goblin here
    ... 

    public bool Attack(IEntity target)
    {
        // change the current entity based on the event
        if (this.IsHungry())
        {
            this.AttackPower--;
        }

        // but let the worldstate handle the actual calculations
        WorldState.Attack(this, target);
    }
}

public WorldState
{
    // other properties of the worldsatate

    public MoveTo(IEntity entityToMove, Location destination)
    {
        // for example
    }

    public Attack(IEntity source, IEntity target)
    {
        // this can become very complex as more tags are added
        // thats why we have a single method to calculate this type of thing
        // in the worldstate and not somewhere as part of the actual Entity
        // these types of calculations can also be 
        // cached/multi-threaded to improve speed
        var attacker = source as SomethingThatCanAttack;
        if (attacker != null)
        {
            var defender = target as SomethingWithDefence;
            if (defender != null)
            {
                if (attacker.AttackPower > defender.Defence)
                {
                    var hard = defender.GetTag<Hard>()
                    if (hard)
                    {
                        if (hard.CanBreak(attacker.AttackPower))
                        {
                            defender.Life = 0;
                        }
                    }
                    else
                    {
                        defender.Life--;
                    }
                }
                else
                {
                    attacker.Life--;
                }
            }
        }

        if (source.Life <= 0)
        {
            source.Destroy();
        }

        if (target.Life <= 0)
        {
            target.Destroy();
        }

    }
}

```
