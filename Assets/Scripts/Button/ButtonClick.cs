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
    }

    public List<ButtonSceneMapping> buttons;

    void Start()
    {
        foreach (ButtonSceneMapping mapping in buttons)
        {
            mapping.button.onClick.AddListener(() => StartScene(mapping.sceneName));
        }
    }

    private void StartScene(string scene)
    {
        if (SceneManager.GetActiveScene().name == scene)
            return;

        SceneManager.LoadScene(scene);
    }
}