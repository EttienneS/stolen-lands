using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class AllTraits
{
    public static Trait AgressiveFaction = new Trait("AgressiveFaction", new List<ActorAction>
        {
            ClaimCell.Agressive, 
            ClaimCell.Default
        });

        public static Trait CautiousFaction = new Trait("CautiousFaction", new List<ActorAction>
        {
            ClaimCell.Cautious, 
            ClaimCell.Default
        });

        public static Trait DefaultFaction = new Trait("DefaultFaction", new List<ActorAction>
        {
            ClaimCell.Agressive, 
            ClaimCell.Cautious, 
            ClaimCell.Default
        });

}