# Main Game Ideas

# Graphics

## Map Meshes

Change mesh to render individual hexes (allows us better control)

## Map Layers

"Known" Map vs Actual Map vs Unexplored Map
Display only known hexes
As hexes are discovered/confirmed populate

## Note: No actual unit representation
### 
## Actor UI Some kind of Actor UI

General UI for all Actor Information.

Current thinking is to go with something akin to CK2 but further research is required.

----------------------

# Actor

## Actor Model

We need some kind of template to what all Actors would conform.  An actor by definition should be something that has the potential to influence the world.  Here is a rough outline of current thinking around the actor model:

### **Race/Type:**

Race determines certain base level things about an actor.

A human fighter would differ from an Orc figher in the sense that an Orc fighter may have a higher baseline Physical score.

Certain races could impact available actions that the Actor can take. 

In programing terms this would be a base class with the 'class' being a more general template applied on top of the Race.

Examples: Human, Elf, Dragon, Orc, Location etc

### **Class:**

Class determines what abilities something has.  This would be a template applied on top of the base Actor (Race).

A fighter would have improved martial, a rogue improved cunning while a mage template would add access to more potential actions.

Examples: 

- Combat roles:   Fighter, mage, rogue, cleric, etc.
- Economic roles: Banker, Farmer, Mayor
- Location roles: Town, Trade Post, Fort, Temple
- Other Roles:### Werewolf, Ghost

### **Traits:**

Descriptions gained by actions/innate.  These can potentially be hidden discovered by other Actors.

Examples:

- **Physical**: Blind, Hunchback, Dwarf, Tall, Burly
- **Virtue/Vice**: Greedy, Proud, Kind, Generous etc.
- **Belief, fear, prejudice**: Hates Orcs, Detests Undead, Fears Dragons, Distrusts Elves 
 
### **Stats:**

- **Physical:**
    - Governs:
        - Moving around the world 
        - HP, 
        - Longevity, 
        - Govern martial combat, 
        - Manual labor
    - Professions: 
        - Farming, 
        - Mining,
        - Soldiering

- **Cunning:**
    - Governs:
        - Perception, 
        - survival skills, 
        - sneaky (spy?), 
        - thievery, 
        - skilled labor 
    - Professions:
        - Thief, 
        - Weaver,
        - Hunter,
        - Builder

- **Mental:**
    - Governs:
        - Gullibility,
        - Performance (genrally a more intelligent character is just a flat persentage better at things) 
        - Govern magic combat, 
    - Professions:
        - Engineer, 
        - accountant, 
        - doctor

- **Charisma:**
    - Governs: 
        - Interpersonal relations, 
        - Lying, 
        - Reading people, 
        - Influencing people, 
    - Professions:
        - Mayor, 
        - Manager,
        - Captain

### Alignment:

Alignment governs the behaviour of actors and is calculated based on the actions of players.

Alignment is judged on two axes:

- Morality: 
    - Good > Neutral > Bad
- Lawfulness: 
    - Chaotic > Neutral > Lawful

## Actor Actions

All actors can take actions, actions are based on several factors of the actor.

## Example:
### Actor:

- Type:   Location
- Class:  Village
- Traits: Starving
- Assets: Livestock (100), Wood (50)  # special sub category gained from the Location Type
- Population: 50 # special sub category gained from the Village class

### Available Actions:
- Slaughter Livestock (gain food)
- Implement food rations (lower food usage, reduce happiness)
- Build Structure (from list of available structures)
- Resolve Event: Angry Peasants
    - Quell Uprising by force (++peasant unhappiness, lawful action)
    - Give peasants some of my food (++peasant happiness, chaotic good action)
    - Ignore issue (neutral action)

### Potential other actions:
- Order actor to do something
- Recruit named Character
- Some kind of influence over the character
- Move somewhere
- Pathfinding 
    - Survival/tracking ability
    - Mounted, flying?
- Some UI of outstanding Order 
    - What is everyone doing?
    - When is it done?  

### Event System

- Any actor can recieve events that have to be resolved on their turn.
- Encouters should be resolved by Dice rolls and context descisions.
- AI Actors would generally take the action governed by their alignment and any other relevant factors (standing orders, traits)

### Open questions on the player vs other actors:
- Profession?
    - Limit to Civilian?
- Traits
- What do I know?
- Map knowledge
    - Visiting hex and mapping yourself gives accurate info
- Hex knowledge
    - Imperfect knowledge?
        - Scout lied about hex content
        - Scout is dumb/incompetent
        - we need some kind of way to determine the verasity of knowledge and if imperfect knowledge affected something we need a way to know how/why
- Endgame based on actions taken
- Is the player a Family/group of characters/faction?
    - Lineage
    - Succession

### Note: Consider implication of Named vs Unnamed actors
- Is every farmer/footsoldier an actor or is an actor only the special most units and the common folk are just numbers/abstractions?

----------------------

# Other Considerations:

### Time per turn

- days, weeks, months
- weights per action


## Relationships
- Spies (mense wat try uit figure wat ander rerig dink)
- Named actors vs alle Named actors
- Not completely known
    - mechanic hoe weet mens wat iemand dink van jou
    - trait discovery

## Factions
- Player is altyd 'n faction
    - Hoe succession 'n player party?
- Faction Succession
    - Faction eliminated if no succession possible
- Motivation
    - Primary goal
    - Affects affiliated actor's goals
- Alliances/Enemies
    - Between factions
    - Members van jou faction breek af en doen hulle eie ding

## Win Condition
- Become King?
- Stay King for X number of years

## Failure Condition
- Character/Lineage death?

## Late game?
- Events based on character/lineage history
    - Uprising if too cruel cruel?
    - Realm schism if too kind?
