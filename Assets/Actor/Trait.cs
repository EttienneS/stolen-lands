using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Trait
{
    public List<ActorAction> Actions {get;set;}

    public string Name {get;set;}

    public Trait(string name, List<ActorAction> actions )
    {
        Name = name;
        Actions = actions; 
    }
}