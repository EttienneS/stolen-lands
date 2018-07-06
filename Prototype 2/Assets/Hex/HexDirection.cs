using System;

[Flags]
public enum HexDirection
{
    NE,
    E,
    SE,
    SW,
    W,
    NW,
}

public static class HexDirectionExtensions
{
    public const HexDirection AllFaces = HexDirection.NE | HexDirection.E | HexDirection.SE | HexDirection.SW | HexDirection.W;

    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? direction + 3 : direction - 3;
    }
}