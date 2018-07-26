using System.Collections.Generic;
using UnityEngine;

public class InfoController : MonoBehaviour
{
    public InfoBox InfoBoxPrefab;

    public Canvas UICanvas;

    private List<InfoBox> _boxes = new List<InfoBox>();

    public void ShowInfoBox(string title, string body, Vector2? position = null)
    {
        var box = Instantiate(InfoBoxPrefab, UICanvas.transform);
        box.SetText(title, body);

        if (position.HasValue)
        {
            box.transform.position = position.Value;
        }
        else
        {
            box.transform.position = new Vector2(box.transform.position.x, box.transform.position.y - (_boxes.Count * 30));
        }

        _boxes.Add(box);
    }

    public void Update()
    {
        // purge closed boxes from the list
        _boxes.RemoveAll(b => b == null);
    }
}
