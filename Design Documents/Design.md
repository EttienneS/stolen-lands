# Stolen Lands

## Overview:

Game inspired by Pathfinder adventure path: Kingmaker.  In running Kingmaker we ran a side game where a player controls all the bandits in the word to create a more immersive experience for the party.

This project is an attempt to flesh this part of the game out into a coherent, playable experience.

## Project goals:

The goal of this project is to learn and improve as programmers by creating something new.

* Learn
* Use best practice (Extreme programming, do not re-invent the wheel)
* Use recent tech
* Ship something (Be AGILE)

## Game pitch:

*The game is a turn based strategy-managment game with some RPG elements.  Manage a fledgeling empire and bring civilization to the wilderness.*

## Gameplay:

### Core gameplay goals:

* Turn-based, at any time you can just stand up and leave if a baby is crying.

* Hex based map a la Civ 5/6

* Tab based UI (Crusader Kings, Europa Universalis style)

* [Entity](./Entity_Design.md) Everything at its lowest level is an entity.  This is to ensure we go with a truely object oriented design.
    * Every agent (see below) is an entity but not every entity is an agent.
    * Every 'thing' in the world should be an entity.  Even abstracted 'concepts' like gold and food should be tied to an actual entity (for example you have 100 gold coin 'entities' that is equivalent in value to 100 gold).
    * Each entity should have an inherent value, 1 gold coin is worth 1, 1 sword is worth 10 etc. this is then tied to the the resource system.
    * Entities as a base type should be scalable to allow for the potentially massive amounts without significant performance overhead (investigate partial object loading)

* [Agent](./Agent_Design.md)  Every thing that can be influenced should be an agent with agency in the world:
    * Cities (Abstracted grouping of entities and agents)
        * Produce goods
        * Procuce heroes
        * Generate quests
        * Attract monsters
        * More happiness, food, services to grow.
    * Heroes
        * Powerful individuals that are the primary force
        * Underlings, less powerful heroes that are lead by others, can become full heroes.
        * Gains levels, equipment
    * Monsters
        * Primary antagonists
        * Attacks cities to steal resources
        * Attack and kill/kidnap heroes
        * Expand and make more monsters (goblins and orcs)
    * etc.

* [Context](./Context_Design.md) Unreliable information should be very important, instant communication is not available.
    * The monster was spotted here last but where is it now?
    * I sent the hero to explore the temple weeks ago what happened?
    * The town on the border has not sent tribute in a while, why?
    * The tax collector only returned with half the normal taxes and says he was robbed, was he or is he a thief?
    * A powerful champion arrives out of nowhere to help you, is he for real or a spy?
    * Viligers keep dissapearing, the local priest accuses a witch, is it her or the priest who is a secret vampire himself?

* Makro management over micro managment.  We want to make the big decision but leave the small desicions to the 'agents'.  For example: 
    * GOOD: Should a village plant corn or wheat? 
    * BAD: Should Jack Matthews plant the field or Matt Jackhews?  
    * GOOD: Should the hero explore the abandoned mine or the ancient temple?
    * BAD: Should the hero go left at the trap or right at the door?

* Events should be random but not _unpredictable_.  The 'game' part should be to use the correct tools for the job. For Example:
    * A tax collector with a 'Drunkard' or 'Gambler' property should be more inclined to steal but it should not be a sureity.
    * A hero with a 'Brave' or 'Bold' property should be less likely to flee but it should still be possible.

* Ignore or manage as you want (Agents should have actual agency)
    * Events should have a default outcome if ignored
    * Agents should have a default behaviour if no order is given
    * Game should be able to play itself if you just leave it (albeit not as efectively)
    
* Things to avoid:
    * *Punishing random states* For example: 
        * Your level 100 hero dies because he got the one 0.1% event that just does an insta kill.
        * You lose a lot of gold to some entirely unpredictable event, if it was foreshadowed then cool but just being punished out of the blue because some random number genrator bombed sucks.
    * *Excessive UI*.  This type of game loves to bombard you with a ton of flowery text for each event or has a thousand buttons and menus all over.  As a rule the game state should be able to comprehended quickly and easily.
    * *Scope creep*.  Maybe we should add some way of playing as a hero?  Maybe we should add a religion mechanic?  Maybe we should add trade routes?  All of these are cool but go agains the prinicple to Ship Something.  We should try to go with an Agile approach and create the 'least shippable' version first and then add more.

## Art:

### Core Art Goals:

* To keep the overhead as low as possible we should try to make due with 'programmer art' for the most part.  This should be designed to be replaceable if we ever want to do that.
* In keeping with the purpose of learning we are going to focus a lot on procedural generation of content.  We want to improve as programmers not sit around designing monsters the whole time.  As a rule, if something is made it should be made automatically.

## Technology

### Core Technology goals:

* ASP.net with WebForms hosted in Azure.  
* Agile development.
* Extreme programming.

## Core Gameplay Loop

1)  Turn Starts
2)  Calculate income
3)  Trigger events for agents
4)  Accept agent inputs
5)  Turn complets
6)  Calculate outcomes for agent actions