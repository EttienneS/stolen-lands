﻿using UnityEngine;

// container class for all the static info about our hexes
public static class HexMetrics
{
    // hex grid based of tutorial found here:
    // https://catlikecoding.com/unity/tutorials/hex-map/part-1/

    // radius of the circle outside the hex (distance from the centre to each corner of the hex)
    public const float OuterRadius = 2.5f;

    // radius of the circle inside the hex (distance from the centre to each edge of the hex, see link above for how this is calculated)
    public const float InnerRadius = OuterRadius * 0.866025404f;

    // the six corners of the hex (sevenths is to wrap back around)
    public static Vector3[] Corners =
    {
        new Vector3(0f, OuterRadius),
        new Vector3(InnerRadius, 0.5f * OuterRadius),
        new Vector3(InnerRadius, -0.5f * OuterRadius),
        new Vector3(0f, -OuterRadius),
        new Vector3(-InnerRadius, -0.5f * OuterRadius),
        new Vector3(-InnerRadius, 0.5f * OuterRadius),
        new Vector3(0f, OuterRadius)
    };
}