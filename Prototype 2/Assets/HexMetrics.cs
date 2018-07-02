﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// container class for all the static info about our hexes
public static class HexMetrics
{
    // hex grid based of tutorial found here:
    // https://catlikecoding.com/unity/tutorials/hex-map/part-1/

    // radius of the circle outside the hex (distance from the centre to each corner of the hex)
    public const float outerRadius = 10f;

    // radius of the circle inside the hex (distance from the centre to each edge of the hex, see link above for how this is calculated)
    public const float innerRadius = outerRadius * 0.866025404f;

    // the six corners of the hex
    public static Vector2[] corners = {
        new Vector2(0f,  outerRadius),
        new Vector2(innerRadius,  0.5f * outerRadius),
        new Vector2(innerRadius,  -0.5f * outerRadius),
        new Vector2(0f,  -outerRadius),
        new Vector2(-innerRadius,  -0.5f * outerRadius),
        new Vector2(-innerRadius,  0.5f * outerRadius),
        new Vector2(0f,  outerRadius),
    };
}
