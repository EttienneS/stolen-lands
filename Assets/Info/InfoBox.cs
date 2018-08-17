using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public string BodyText;
    public string TitleText;

    public void Awake()
    {
        SetText(TitleText, BodyText);
    }

    public void SetText(string title, string body)
    {
        TitleText = title;
        BodyText = body;

        transform.Find("Title").GetComponent<Text>().text = TitleText;
        transform.Find("Body").GetComponent<Text>().text = BodyText;
    }
}