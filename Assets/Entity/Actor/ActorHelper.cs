using UnityEngine;

public class ActorHelper
{
    public static string GetRandomName()
    {
        // very basic name generator, just combines the three parts together
        // todo: Expand this to have some start letters be more or less common (for example X names are more rare)
        // todo: Add some kind of racial filter, perhaps a dragon has a longer name and elves have the classic el'ven style apostrophe names
        var front = new[]
        {
            "Ch", "K", "Sh", "R", "S", "St", "B", "T", "X", "P", "D", "Kr", "Can", "Ex", "J", "H", "Th", "Sch", "Ten",
            "W", "Wr", "V"
        };
        var mid = new[] { "a", "e", "u", "olo", "i", "o", "oo", "ee", "ero", "ane", "ala", "are", "ou", "y", "ai" };
        var end = new[]
        {
            "ll", "xel", "lle", "p", "ck", "t", "ne", "lla", "le", "x", "lo", "lee", "bel", "tel", "xa", "ty", "te",
            "se"
        };

        var name = string.Empty;

        name += front[Random.Range(0, front.Length - 1)];
        name += mid[Random.Range(0, mid.Length - 1)];
        name += end[Random.Range(0, end.Length - 1)];

        return name;
    }
}