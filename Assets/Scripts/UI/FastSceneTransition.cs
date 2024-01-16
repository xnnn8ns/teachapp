using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FastSceneTransition : MonoBehaviour
{
    [SerializeField] private SceneTransition sceneTransition;
    private string previousSceneName;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartFastSceneTransition(string sceneName)
    {
        sceneTransition.StartTransition();
        previousSceneName = GetActiveSceneName();
        StartCoroutine(FastTransitionToScene(sceneName));
    }

    IEnumerator FastTransitionToScene(string sceneName)
    {
        yield return StartCoroutine(FastFadeIn());

        if (sceneName == "MapScene")
        {
            sceneTransition.CatImage.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            sceneTransition.CatImage.transform.parent.gameObject.SetActive(true);
        }

        StartScene(sceneName, false, true);

        yield return StartCoroutine(FadeOut());

        yield return new WaitForSeconds(0.5f);

        if (!string.IsNullOrEmpty(previousSceneName) && previousSceneName != "MapScene" && SceneManager.GetSceneByName(previousSceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(previousSceneName);
        }
    }

    IEnumerator FastFadeIn()
    {
        for (float t = 0.01f; t < 1f; t += Time.deltaTime)
        {
            if (sceneTransition.TransitionImage == null || sceneTransition.CatImage == null)
            {
                yield break;
            }
            Color newColor = new Color(sceneTransition.TransitionImage.color.r, sceneTransition.TransitionImage.color.g, sceneTransition.TransitionImage.color.b, Mathf.Lerp(0f, 1f, t));
            sceneTransition.TransitionImage.color = newColor;
            sceneTransition.CatImage.color = newColor;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        for (float t = 0.01f; t < 1f; t += Time.deltaTime)
        {
            Color newColor = new Color(sceneTransition.TransitionImage.color.r, sceneTransition.TransitionImage.color.g, sceneTransition.TransitionImage.color.b, Mathf.Lerp(1f, 0f, t));
            sceneTransition.TransitionImage.color = newColor;
            sceneTransition.CatImage.color = newColor;
            yield return null;
        }
    }

    public void StartScene(string scene, bool isSingle, bool isNeedBeClosed)
    {
        if (string.IsNullOrEmpty(scene)) return;

        if (isSingle)
        {
            StartCoroutine(LoadSceneAsync(scene, LoadSceneMode.Single));
            previousSceneName = scene;
        }
        else
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
            }
            else
            {
                StartCoroutine(LoadSceneAsync(scene, LoadSceneMode.Additive));
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
            }
            previousSceneName = scene;
        }
    }

    IEnumerator LoadSceneAsync(string scene, LoadSceneMode mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, mode);

        // Ждем, пока сцена полностью не загрузится
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    string GetActiveSceneName()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if (loadedScene.isLoaded)
            {
                return loadedScene.name;
            }
        }
        return null;
    }
}