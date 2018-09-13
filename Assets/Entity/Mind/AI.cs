using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : Mind
{
    public override void Act()
    {
        Console.WriteLine("This is a test");

        foreach (var action in Entity.AvailableActions)
        {

        }
    }
}