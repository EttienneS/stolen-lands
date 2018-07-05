using System;
using UnityEngine;

[Serializable]
public struct HexCoordinates
{
    [SerializeField] private readonly int x;

    [SerializeField] private readonly int y;

    public int X
    {
        get { return x; }
    }

    public int Y
    {
        get { return y; }
    }

    public HexCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float z = -x;

        float offset = position.y / (HexMetrics.outerRadius * 3f);
        x -= offset;
        z -= offset;

        int iX = Mathf.RoundToInt(x);
        int iZ = Mathf.RoundToInt(z);
        int iY = Mathf.RoundToInt(-x - z);

        if (iX + iZ + iY != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dZ = Mathf.Abs(z - iZ);
            float dY = Mathf.Abs(-x - z - iY);

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
}