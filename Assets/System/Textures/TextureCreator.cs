using Boo.Lang;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    public Gradient coloring;

    [Range(1, 3)] public int dimensions = 3;

    public float frequency = 1f;

    [Range(1f, 4f)] public float lacunarity = 2f;

    public bool Live = false;

    [Range(1, 8)] public int octaves = 1;

    [Range(0f, 1f)] public float persistence = 0.5f;

    [Range(2, 512)] public int resolution = 256;

    private Texture2D texture;

    public NoiseMethodType type;

    private void OnEnable()
    {
        if (texture == null)
        {
            texture = TextureHelper.CreateTexture(resolution);
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }

        FillThisTexture();
    }

    private void Update()
    {
        if (Live)
        {
            if (transform.hasChanged)
            {
                transform.hasChanged = false;
            }

            FillThisTexture();
        }
    }

    public void FillThisTexture()
    {
        var point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        var point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        var point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        var point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        texture = TextureHelper.FillTexture(texture, type, coloring,
            point00, point10, point01, point11,
            resolution, dimensions, frequency, octaves, lacunarity, persistence);
    }

    public static Texture2D GetTexture(Transform transform = null, int resolution = 64, Color? dominantColor = null)
    {
        Vector3 point00, point10, point01, point11;

        if (transform != null)
        {
            point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
            point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
            point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
            point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));
        }
        else
        {
            point00 = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
            point10 = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
            point01 = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
            point11 = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
        }

        var noteGradient = new Gradient();

        var gradient = new List<GradientColorKey>();

        if (dominantColor.HasValue)
        {
            // if the dominant color is set use it along with randoms
            // the dominant color has the widest band so it should be the most pronounced
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 0f));
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 0.2f));
            gradient.Add(new GradientColorKey(dominantColor.Value, 0.5f));
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 0.8f));
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 1f));
        }
        else
        {
            // if the dominant color is not set use purely random values
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 0));
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 0.25f));
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 0.5f));
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 0.75f));
            gradient.Add(new GradientColorKey(TextureHelper.GetRandomColor(), 1));
        }

        noteGradient.SetKeys(gradient.ToArray(),
            new[]
            {
                new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1)
            });

        return TextureHelper.FillTexture(TextureHelper.CreateTexture(resolution), NoiseMethodType.Value, noteGradient,
            point00, point10, point01,
            point11, resolution);
    }
}

public class TextureHelper
{
    public static Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public static Texture2D CreateTexture(int resolution)
    {
        var texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true)
        {
            name = "Procedural Texture",
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Trilinear,
            anisoLevel = 9
        };

        return texture;
    }

    public static Texture2D FillTexture(Texture2D texture, NoiseMethodType type, Gradient coloring,
        Vector3 point00, Vector3 point10, Vector3 point01, Vector3 point11,
        int resolution = 64, int dimensions = 2, float frequency = 2f, int octaves = 3, float lacunarity = 4f,
        float persistence = 0.5f)
    {
        if (texture.width != resolution)
        {
            texture.Resize(resolution, resolution);
        }

        var stepSize = 1f / resolution;
        for (var y = 0; y < resolution; y++)
        {
            var point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            var point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
            for (var x = 0; x < resolution; x++)
            {
                var point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                var sample = Noise.Sum(Noise.methods[(int)type][dimensions - 1], point, frequency, octaves,
                    lacunarity, persistence);
                if (type != NoiseMethodType.Value)
                {
                    sample = sample * 0.5f + 0.5f;
                }

                texture.SetPixel(x, y, coloring.Evaluate(sample));
            }
        }

        texture.Apply();

        return texture;
    }
}