using UnityEngine;

public delegate float NoiseMethod(Vector3 point, float frequency);

public enum NoiseMethodType
{
    Value,
    Perlin
}

public static class Noise
{
    private const int hashMask = 255;

    private const int gradientsMask1D = 1;

    private const int gradientsMask2D = 7;

    private const int gradientsMask3D = 15;

    public static NoiseMethod[] valueMethods =
    {
        Value1D,
        Value2D,
        Value3D
    };

    public static NoiseMethod[] perlinMethods =
    {
        Perlin1D,
        Perlin2D,
        Perlin3D
    };

    public static NoiseMethod[][] methods =
    {
        valueMethods,
        perlinMethods
    };

    private static readonly int[] hash =
    {
        151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225,
        140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148,
        247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32,
        57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175,
        74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
        60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54,
        65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169,
        200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64,
        52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212,
        207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213,
        119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9,
        129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104,
        218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241,
        81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157,
        184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93,
        222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180,

        151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225,
        140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148,
        247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32,
        57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175,
        74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
        60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54,
        65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169,
        200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64,
        52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212,
        207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213,
        119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9,
        129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104,
        218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241,
        81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157,
        184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93,
        222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
    };

    private static readonly float[] gradients1D =
    {
        1f, -1f
    };

    private static readonly Vector2[] gradients2D =
    {
        new Vector2(1f, 0f),
        new Vector2(-1f, 0f),
        new Vector2(0f, 1f),
        new Vector2(0f, -1f),
        new Vector2(1f, 1f).normalized,
        new Vector2(-1f, 1f).normalized,
        new Vector2(1f, -1f).normalized,
        new Vector2(-1f, -1f).normalized
    };

    private static readonly Vector3[] gradients3D =
    {
        new Vector3(1f, 1f, 0f),
        new Vector3(-1f, 1f, 0f),
        new Vector3(1f, -1f, 0f),
        new Vector3(-1f, -1f, 0f),
        new Vector3(1f, 0f, 1f),
        new Vector3(-1f, 0f, 1f),
        new Vector3(1f, 0f, -1f),
        new Vector3(-1f, 0f, -1f),
        new Vector3(0f, 1f, 1f),
        new Vector3(0f, -1f, 1f),
        new Vector3(0f, 1f, -1f),
        new Vector3(0f, -1f, -1f),

        new Vector3(1f, 1f, 0f),
        new Vector3(-1f, 1f, 0f),
        new Vector3(0f, -1f, 1f),
        new Vector3(0f, -1f, -1f)
    };

    private static readonly float sqr2 = Mathf.Sqrt(2f);

    private static float Dot(Vector2 g, float x, float y)
    {
        return g.x * x + g.y * y;
    }

    private static float Dot(Vector3 g, float x, float y, float z)
    {
        return g.x * x + g.y * y + g.z * z;
    }

    private static float Smooth(float t)
    {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    public static float Value1D(Vector3 point, float frequency)
    {
        point *= frequency;
        var i0 = Mathf.FloorToInt(point.x);
        var t = point.x - i0;
        i0 &= hashMask;
        var i1 = i0 + 1;

        var h0 = hash[i0];
        var h1 = hash[i1];

        t = Smooth(t);
        return Mathf.Lerp(h0, h1, t) * (1f / hashMask);
    }

    public static float Value2D(Vector3 point, float frequency)
    {
        point *= frequency;
        var ix0 = Mathf.FloorToInt(point.x);
        var iy0 = Mathf.FloorToInt(point.y);
        var tx = point.x - ix0;
        var ty = point.y - iy0;
        ix0 &= hashMask;
        iy0 &= hashMask;
        var ix1 = ix0 + 1;
        var iy1 = iy0 + 1;

        var h0 = hash[ix0];
        var h1 = hash[ix1];
        var h00 = hash[h0 + iy0];
        var h10 = hash[h1 + iy0];
        var h01 = hash[h0 + iy1];
        var h11 = hash[h1 + iy1];

        tx = Smooth(tx);
        ty = Smooth(ty);
        return Mathf.Lerp(
                   Mathf.Lerp(h00, h10, tx),
                   Mathf.Lerp(h01, h11, tx),
                   ty) * (1f / hashMask);
    }

    public static float Value3D(Vector3 point, float frequency)
    {
        point *= frequency;
        var ix0 = Mathf.FloorToInt(point.x);
        var iy0 = Mathf.FloorToInt(point.y);
        var iz0 = Mathf.FloorToInt(point.z);
        var tx = point.x - ix0;
        var ty = point.y - iy0;
        var tz = point.z - iz0;
        ix0 &= hashMask;
        iy0 &= hashMask;
        iz0 &= hashMask;
        var ix1 = ix0 + 1;
        var iy1 = iy0 + 1;
        var iz1 = iz0 + 1;

        var h0 = hash[ix0];
        var h1 = hash[ix1];
        var h00 = hash[h0 + iy0];
        var h10 = hash[h1 + iy0];
        var h01 = hash[h0 + iy1];
        var h11 = hash[h1 + iy1];
        var h000 = hash[h00 + iz0];
        var h100 = hash[h10 + iz0];
        var h010 = hash[h01 + iz0];
        var h110 = hash[h11 + iz0];
        var h001 = hash[h00 + iz1];
        var h101 = hash[h10 + iz1];
        var h011 = hash[h01 + iz1];
        var h111 = hash[h11 + iz1];

        tx = Smooth(tx);
        ty = Smooth(ty);
        tz = Smooth(tz);
        return Mathf.Lerp(
                   Mathf.Lerp(Mathf.Lerp(h000, h100, tx), Mathf.Lerp(h010, h110, tx), ty),
                   Mathf.Lerp(Mathf.Lerp(h001, h101, tx), Mathf.Lerp(h011, h111, tx), ty),
                   tz) * (1f / hashMask);
    }

    public static float Perlin1D(Vector3 point, float frequency)
    {
        point *= frequency;
        var i0 = Mathf.FloorToInt(point.x);
        var t0 = point.x - i0;
        var t1 = t0 - 1f;
        i0 &= hashMask;
        var i1 = i0 + 1;

        var g0 = gradients1D[hash[i0] & gradientsMask1D];
        var g1 = gradients1D[hash[i1] & gradientsMask1D];

        var v0 = g0 * t0;
        var v1 = g1 * t1;

        var t = Smooth(t0);
        return Mathf.Lerp(v0, v1, t) * 2f;
    }

    public static float Perlin2D(Vector3 point, float frequency)
    {
        point *= frequency;
        var ix0 = Mathf.FloorToInt(point.x);
        var iy0 = Mathf.FloorToInt(point.y);
        var tx0 = point.x - ix0;
        var ty0 = point.y - iy0;
        var tx1 = tx0 - 1f;
        var ty1 = ty0 - 1f;
        ix0 &= hashMask;
        iy0 &= hashMask;
        var ix1 = ix0 + 1;
        var iy1 = iy0 + 1;

        var h0 = hash[ix0];
        var h1 = hash[ix1];
        var g00 = gradients2D[hash[h0 + iy0] & gradientsMask2D];
        var g10 = gradients2D[hash[h1 + iy0] & gradientsMask2D];
        var g01 = gradients2D[hash[h0 + iy1] & gradientsMask2D];
        var g11 = gradients2D[hash[h1 + iy1] & gradientsMask2D];

        var v00 = Dot(g00, tx0, ty0);
        var v10 = Dot(g10, tx1, ty0);
        var v01 = Dot(g01, tx0, ty1);
        var v11 = Dot(g11, tx1, ty1);

        var tx = Smooth(tx0);
        var ty = Smooth(ty0);
        return Mathf.Lerp(
                   Mathf.Lerp(v00, v10, tx),
                   Mathf.Lerp(v01, v11, tx),
                   ty) * sqr2;
    }

    public static float Perlin3D(Vector3 point, float frequency)
    {
        point *= frequency;
        var ix0 = Mathf.FloorToInt(point.x);
        var iy0 = Mathf.FloorToInt(point.y);
        var iz0 = Mathf.FloorToInt(point.z);
        var tx0 = point.x - ix0;
        var ty0 = point.y - iy0;
        var tz0 = point.z - iz0;
        var tx1 = tx0 - 1f;
        var ty1 = ty0 - 1f;
        var tz1 = tz0 - 1f;
        ix0 &= hashMask;
        iy0 &= hashMask;
        iz0 &= hashMask;
        var ix1 = ix0 + 1;
        var iy1 = iy0 + 1;
        var iz1 = iz0 + 1;

        var h0 = hash[ix0];
        var h1 = hash[ix1];
        var h00 = hash[h0 + iy0];
        var h10 = hash[h1 + iy0];
        var h01 = hash[h0 + iy1];
        var h11 = hash[h1 + iy1];
        var g000 = gradients3D[hash[h00 + iz0] & gradientsMask3D];
        var g100 = gradients3D[hash[h10 + iz0] & gradientsMask3D];
        var g010 = gradients3D[hash[h01 + iz0] & gradientsMask3D];
        var g110 = gradients3D[hash[h11 + iz0] & gradientsMask3D];
        var g001 = gradients3D[hash[h00 + iz1] & gradientsMask3D];
        var g101 = gradients3D[hash[h10 + iz1] & gradientsMask3D];
        var g011 = gradients3D[hash[h01 + iz1] & gradientsMask3D];
        var g111 = gradients3D[hash[h11 + iz1] & gradientsMask3D];

        var v000 = Dot(g000, tx0, ty0, tz0);
        var v100 = Dot(g100, tx1, ty0, tz0);
        var v010 = Dot(g010, tx0, ty1, tz0);
        var v110 = Dot(g110, tx1, ty1, tz0);
        var v001 = Dot(g001, tx0, ty0, tz1);
        var v101 = Dot(g101, tx1, ty0, tz1);
        var v011 = Dot(g011, tx0, ty1, tz1);
        var v111 = Dot(g111, tx1, ty1, tz1);

        var tx = Smooth(tx0);
        var ty = Smooth(ty0);
        var tz = Smooth(tz0);
        return Mathf.Lerp(
            Mathf.Lerp(Mathf.Lerp(v000, v100, tx), Mathf.Lerp(v010, v110, tx), ty),
            Mathf.Lerp(Mathf.Lerp(v001, v101, tx), Mathf.Lerp(v011, v111, tx), ty),
            tz);
    }

    public static float Sum(NoiseMethod method, Vector3 point, float frequency, int octaves, float lacunarity,
        float persistence)
    {
        var sum = method(point, frequency);
        var amplitude = 1f;
        var range = 1f;
        for (var o = 1; o < octaves; o++)
        {
            frequency *= lacunarity;
            amplitude *= persistence;
            range += amplitude;
            sum += method(point, frequency) * amplitude;
        }

        return sum / range;
    }
}