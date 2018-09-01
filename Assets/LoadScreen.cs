using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    void Update()
    {
        if (!_loading)
        {
            _loading = true;
            StartCoroutine(LoadNewScene());
            LoadNewScene();
        }
        else
        {
            var panel = GameObject.Find("LoadPanel").GetComponent<Image>();
            panel.color = new Color(0f, Random.Range(0.4f, 0.6f), 0f);
        }
    }

    private bool _loading;

    public static IEnumerator LoadNewScene()
    {
        var async = SceneManager.LoadSceneAsync("Main");
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
