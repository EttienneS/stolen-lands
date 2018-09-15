using System;
using UnityEngine;

[Serializable]
public struct HexCoordinates
{
    [SerializeField] private readonly int x;

    [SerializeField] private readonly int y;

    public int X => x;

    public int Y => y;

    public int Z => -X - Y;

    public HexCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        var x = position.x / (HexMetrics.InnerRadius * 2f);
        var z = -x;

        var offset = position.y / (HexMetrics.OuterRadius * 3f);
        x -= offset;
        z -= offset;

        var iX = Mathf.RoundToInt(x);
        var iZ = Mathf.RoundToInt(z);
        var iY = Mathf.RoundToInt(-x - z);

        if (iX + iZ + iY != 0)
        {
            var dX = Mathf.Abs(x - iX);
            var dZ = Mathf.Abs(z - iZ);
            var dY = Mathf.Abs(-x - z - iY);

            if (dX > dZ && dX > dY)
            {
                iX = -iZ - iY;
            }
            else if (dY > dZ)
            {
                iY = -iX - iZ;
            }
        }

        return new HexCoordinates(iX, iY);
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new HexCoordinates(x - y / 2, y);
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X + "\n" + Y;
    }

    public int DistanceTo(HexCoordinates other)
    {
        return ((x < other.x ? other.x - x : x - other.x) +
                (Y < other.Y ? other.Y - Y : Y - other.Y) +
                (Z < other.Z ? other.Z - Z : Z - other.Z)) / 2;
    }
}