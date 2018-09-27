using System.IO;
using UnityEngine;

public static class SceneData
{
    public static bool LoadMode { get; set; }

    public static string SaveFolder = Path.Combine(Application.persistentDataPath, "Save");
}