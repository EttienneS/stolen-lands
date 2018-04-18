# Context

## Overview:

The concept of unreliable or delayed information is core to the game.  To achieve this we need to keep a seperate information state for each Entity.  For example:

* A goblin would care about the location of settlements but not really have much knowledge about the comings and goings of the local tax collector.
* A monster hunter hero would care about the lair of a dragon but not care about the lair of a goblin tribe as much.
* [Agent](./Agent_Design.md) document contains more information about the way agents can gain and share information.
* [Entity](./Entity_Design.md) document contains more information about the way entity information is known.

## Example of the context base context type:

```csharp
interface IContext
{
    List<IKnowledge> KnownInformation {get; set;}       
}

interface IKnowlege
{
    List<IEntity> RelevantEntities {get; set;}
}
```
