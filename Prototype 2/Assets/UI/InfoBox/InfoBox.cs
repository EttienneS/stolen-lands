using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public string TitleText;
    public string BodyText;

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
