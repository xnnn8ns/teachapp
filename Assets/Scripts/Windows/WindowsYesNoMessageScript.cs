using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsYesNoMessageScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textTitle;
    private string _sceneToLoad = "";
    [SerializeField]
    private AudioSource _clickAudio;

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
    }

    //public void FillWindowData(Action actionClickOk, string title)
    //{
    //    _textTitle.text = title;
    //}

    public void ClickOK()
    {
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
    }

    public void ClickCancel()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "WindowYesNowScene")
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
        //foreach (var item in SceneManager.GetAllScenes())
        //{
        //    if (item.name == "WindowYesNowScene")
        //        SceneManager.UnloadSceneAsync(item);
        //}

    }
}
