using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    private bool _loading;

    private void Update()
    {
        if (!_loading)
        {
            _loading = true;
            StartCoroutine(LoadNewScene());
            LoadNewScene();
        }
    }

    public static IEnumerator LoadNewScene()
    {
        var async = SceneManager.LoadSceneAsync("Main");
        while (!async.isDone)
        {
            yield return null;
        }
    }
}