using UnityEngine;

public class InfoBoxCloseButton : MonoBehaviour
{
    public void OnClick()
    {
        Destroy(transform.parent.gameObject);
    }
}