using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBoxCloseButton : MonoBehaviour
{
    public void OnClick()
    {
        Destroy(transform.parent.gameObject);
    }
}
