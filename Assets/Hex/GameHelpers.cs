using UnityEngine;

public static class GameHelpers
{
    public static int UnkownLayer => 8;
    public static int KnownLayer => 9;

    public static int VisibleLayer => 10;



    public static Renderer[] GetAllRenderersForObject(GameObject objectToCheck)
    {
        return objectToCheck.GetComponentsInChildren<Renderer>();
    }

    public static Vector3 CalculateSizeForObject(GameObject objectToMove)
    {
        var size = new Vector3();

        foreach (var renderer in GetAllRenderersForObject(objectToMove))
        {
            size += renderer.bounds.size;
        }

        return size;
    }

    public static void ChangeLayer(GameObject item, int visibleLayer)
    {
        item.layer = visibleLayer;

        foreach (Transform child in item.transform)
        {
            if (child == null)
            {
                continue;
            }

            ChangeLayer(child.gameObject, visibleLayer);
        }
    }
}
