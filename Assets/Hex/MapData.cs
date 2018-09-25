using System.Collections.Generic;
using UnityEngine;

public static class MapData
{
    public enum Type
    {
        Water = 0,
        DeepWater = 1,
        Desert = 2,
        Grassland = 3,
        Forest = 4,
        Mountain = 5
    }

    private const float AlmostNone = 0.01f;
    private const float VeryRare = 0.05f;
    private const float Rare = 0.1f;
    private const float Common = 0.4f;
    private const float Everywhere = 0.8f;


    public static readonly List<List<KeyValuePair<Type, float>>> Recipes = new List<List<KeyValuePair<Type, float>>>
    {
        // basic map
        new List<KeyValuePair<Type, float>>
        {
            new KeyValuePair<Type, float>(Type.Desert, Rare),
            new KeyValuePair<Type, float>(Type.Grassland, Everywhere),
            new KeyValuePair<Type, float>(Type.Forest, Common),
            new KeyValuePair<Type, float>(Type.Water, AlmostNone),
            new KeyValuePair<Type, float>(Type.Mountain, VeryRare),
        },

        //  water world
        //new List<KeyValuePair<Type, float>>
        //{
        //    new KeyValuePair<Type, float>(Type.Desert, Rare)
        //},
    };

    public static readonly Dictionary<Type, CellType> HexTypes = new Dictionary<Type, CellType>
    {
        {Type.Grassland, new CellType(Type.Grassland, new Color(0f, 0.7f, 0f),1)},
        {Type.Forest, new CellType(Type.Forest, new Color(0f, 0.4f, 0f), 2)},
        {Type.Desert, new CellType(Type.Desert, new Color(1, 0.9f, 0.85f), 2)},
        {Type.Mountain, new CellType(Type.Mountain, new Color(0.5f,0.5f,0.5f), -1, 0.01f, 2)},
        {Type.Water, new CellType(Type.Water, new Color(0f, 0f, 0.8f), -1, 0.1f, 0)},
        {Type.DeepWater, new CellType(Type.DeepWater, new Color(0f, 0f, 0.4f), -1, 0.1f, 0)}
    };
}

public class CellType
{
    private readonly float _colorVariance;
    public readonly float MaxElevation;

    public readonly int TravelCost;

    private readonly Color _baseColor;
    public readonly MapData.Type TypeName;

    public CellType(MapData.Type typeName, Color baseColor, int travelCost, float colorVariance = 0.1f,
        float maxElevation = 0.4f)
    {
        TypeName = typeName;
        _baseColor = baseColor;
        TravelCost = travelCost;

        _colorVariance = colorVariance;

        MaxElevation = maxElevation;
    }

    public Color Color => new Color(Mathf.Clamp(_baseColor.r + Random.Range(-_colorVariance, _colorVariance), 0, 1f),
        Mathf.Clamp(_baseColor.g + Random.Range(-_colorVariance, _colorVariance), 0, 1f),
        Mathf.Clamp(_baseColor.b + Random.Range(-_colorVariance, _colorVariance), 0, 1f));
}