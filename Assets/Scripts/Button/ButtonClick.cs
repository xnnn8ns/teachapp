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
            mapping.button.onClick.AddListener(() => SceneManager.LoadScene(mapping.sceneName));
        }
    }
}