using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public GameObject normalScreen;
    public GameObject LoadingScreen;
    public Slider slider;
    public float loadingTime;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        normalScreen.SetActive(false);
        LoadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false;

        float elapsedTime = 0.0f;
        
        while (elapsedTime < loadingTime)
        {
            float progress = Mathf.Clamp01(elapsedTime / loadingTime);
            slider.value = progress;
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            SceneManager.LoadScene("GamePage");

            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;

            yield return null;
        }
        
    }
}
