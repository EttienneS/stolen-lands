using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBoxCloseButton : MonoBehaviour
{

    public void OnPointerDown(PointerEventData data)
    {
        var parent = transform.parent;
        Destroy(parent);
    }
}
