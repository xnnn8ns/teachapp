using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonClickHandler : MonoBehaviour
{
    [System.Serializable]
    public class ButtonSceneMapping
    {
        public Button button;
        public string sceneName;
        public bool isSingle = true;
    }

    public List<ButtonSceneMapping> buttons;

    void Start()
    {
        foreach (ButtonSceneMapping mapping in buttons)
        {
            mapping.button.onClick.AddListener(() => StartScene(mapping.sceneName, mapping.isSingle));
        }
    }

    private void StartScene(string scene, bool isSingle)
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        Debug.Log(scene);
        if (SceneManager.GetActiveScene().name == scene && SceneManager.sceneCount == 1)
            return;
        if (SceneManager.sceneCount > 1 && SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name == scene)
            return;
        bool isSubScene = false;
        Debug.Log("SceneManager.sceneCount " + SceneManager.sceneCount);
        for (int i = SceneManager.sceneCount - 1; i >= 1; i--)
        {
            //if (SceneManager.GetSceneAt(i).name != scene && SceneManager.GetSceneAt(i).isSubScene)
            //{
                Debug.Log(SceneManager.GetSceneAt(i).name);
                isSubScene = SceneManager.GetSceneAt(i).isSubScene;
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                
            //}
        }
        if (isSubScene)
            return;
        if (SceneManager.GetActiveScene().name == scene)
            return;
        Debug.Log("Load " + isSingle);
        if (isSingle)
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        else
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }
}