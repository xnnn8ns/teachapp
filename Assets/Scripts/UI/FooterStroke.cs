using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FooterStroke : MonoBehaviour
{
    [System.Serializable]
    public class SceneImage
    {
        public string sceneName;
        public Image image;
    }

    [SerializeField]
    private List<SceneImage> sceneImages = new List<SceneImage>();
    private string activeSceneName;

    void Start()
    {
        foreach (var sceneImage in sceneImages)
        {
            sceneImage.image.gameObject.SetActive(false);
        }

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        activeSceneName = GetActiveSceneName();
        UpdateImages();
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public void StartScene(string scene, bool isSingle, bool isNeedBeClosed)
    {
        if (isSingle)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
            activeSceneName = scene;
        }
        else
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
            activeSceneName = scene;
        }
        UpdateImages();
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        activeSceneName = newScene.name;
        UpdateImages();
    }

    string GetActiveSceneName()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            if (loadedScene.name != "MapScene" && loadedScene.name != "DontDestroyOnLoad")
            {
                return loadedScene.name;
            }
        }
        return "MapScene";
    }

    void UpdateImages()
    {
        foreach (var sceneImage in sceneImages)
        {
            if (sceneImage.image != null)
            {
                bool isActiveScene = sceneImage.sceneName == activeSceneName;
                sceneImage.image.gameObject.SetActive(isActiveScene);
            }
        }
    }
}