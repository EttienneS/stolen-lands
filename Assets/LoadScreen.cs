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
        else
        {
            var panel = GameObject.Find("LoadPanel").GetComponent<Image>();
            panel.color = new Color(0f, Random.Range(0.4f, 0.6f), 0f);
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