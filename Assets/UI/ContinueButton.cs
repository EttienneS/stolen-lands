using System.IO;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    private void Start()
    {
        if (!Directory.Exists(SceneData.SaveFolder))
        {
            transform.gameObject.SetActive(false);
        }
    }
}