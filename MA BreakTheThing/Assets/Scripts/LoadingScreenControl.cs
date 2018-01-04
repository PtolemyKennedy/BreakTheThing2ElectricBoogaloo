using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenControl : MonoBehaviour
{
    public GameObject loadingScreenObj;
    public Slider slider;

    AsyncOperation async; 

    public void LoadScreen(string sceneName)
    {
        StartCoroutine(LoadingScreen(sceneName)); 
    }

    IEnumerator LoadingScreen(string sceneToLoad)
    {
        loadingScreenObj.SetActive(true);
        async = SceneManager.LoadSceneAsync(sceneToLoad);
        async.allowSceneActivation = false;

        while (async.isDone == false)
        {
            slider.value = async.progress;

            if (async.progress == 0.9f)
            {
                yield return new WaitForSeconds(1.5f);
                slider.value = 1f;
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}