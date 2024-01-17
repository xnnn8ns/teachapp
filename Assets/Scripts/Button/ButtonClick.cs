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
        public bool isNeedBeClosed = true;
        public bool showTransition = false;
    }

    public List<ButtonSceneMapping> buttons;

    void Start()
    {
        foreach (ButtonSceneMapping mapping in buttons)
        {
            mapping.button.onClick.AddListener(() => StartScene(mapping.sceneName, mapping.isSingle, mapping.isNeedBeClosed, mapping.showTransition));
        }
    }

    private void StartScene(string scene, bool isSingle, bool isNeedBeClosed, bool showTransition)
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        Debug.Log(scene);
        if (SceneManager.GetActiveScene().name == scene && SceneManager.sceneCount == 1)
            return;
        if (SceneManager.sceneCount > 1 && SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name == scene)
            return;
        bool isSubScene = false;
        Debug.Log("SceneManager.sceneCount " + SceneManager.sceneCount);
        if (isNeedBeClosed)
        {
            for (int i = SceneManager.sceneCount - 1; i >= 1; i--)
            {
                Debug.Log(SceneManager.GetSceneAt(i).name);
                isSubScene = SceneManager.GetSceneAt(i).isSubScene;
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
        Settings.IsModalWindowOpened = false;
        if (isSubScene)
            return;
        if (SceneManager.GetActiveScene().name == scene)
            return;
        Debug.Log("Load " + isSingle);

        if (showTransition)
        {
            // Загружаем сцену с переходом
            SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
            if (sceneTransition != null)
            {
                sceneTransition.StartSceneTransition(scene);
            }
        }
        else
        {
            // Загружаем сцену обычным способом
            if (isSingle)
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            else
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }
}