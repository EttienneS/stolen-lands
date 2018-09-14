using UnityEngine;

public static class GameHelpers
{
    public static int KnownLayer => 9;
    public static int UnknownLayer => 8;
    public static int VisibleLayer => 10;


    public static Vector3 CalculateSizeForObject(GameObject objectToMove)
    {
        var size = new Vector3();

        foreach (var renderer in GetAllRenderersForObject(objectToMove))
        {
            size += renderer.bounds.size;
        }

        return size;
    }

    public static Renderer[] GetAllRenderersForObject(GameObject objectToCheck)
    {
        return objectToCheck.GetComponentsInChildren<Renderer>();
    }

    public static void MoveToKnownLayer(this GameObject item)
    {
        MoveToLayer(item, KnownLayer);
    }

    public static void MoveToLayer(this GameObject item, int visibleLayer)
    {
        item.layer = visibleLayer;

        foreach (Transform child in item.transform)
        {
            if (child == null)
            {
                continue;
            }

            MoveToLayer(child.gameObject, visibleLayer);
        }
    }

    public static void MoveToUnknownLayer(this GameObject item)
    {
        MoveToLayer(item, UnknownLayer);
    }

    public static void MoveToVisibleLayer(this GameObject item)
    {
        MoveToLayer(item, VisibleLayer);
    }
}