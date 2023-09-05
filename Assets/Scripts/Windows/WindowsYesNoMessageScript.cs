using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsYesNoMessageScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textTitle;

    private Action _actionClickOk;

    public void FillWindowData(Action actionClickOk, string title)
    {
        _textTitle.text = title;
        _actionClickOk = actionClickOk;
    }

    public void ClickOK()
    {
        _actionClickOk.Invoke();
    }

    public void ClickCancel()
    {
        //_actionClickCancel.Invoke();

        foreach (var item in SceneManager.GetAllScenes())
        {
            if (item.name == "WindowYesNowScene")
                SceneManager.UnloadSceneAsync(item);
        }

    }
}
