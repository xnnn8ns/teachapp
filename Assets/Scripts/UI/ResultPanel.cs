using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundPanelOK;
    [SerializeField]
    private GameObject backgroundPanelWrong;
    [SerializeField]
    private TextMeshProUGUI textResult;

    [SerializeField]
    private GameObject imagelOK;
    [SerializeField]
    private GameObject imageWrong;

    private Action action;

    

    public void ShowPanel(bool isOk, string txtResult, Action callBack)
    {
        action = callBack;
        backgroundPanelOK.SetActive(isOk);
        backgroundPanelWrong.SetActive(!isOk);
        imagelOK.SetActive(isOk);
        imageWrong.SetActive(!isOk);
        textResult.text = txtResult;
        //Debug.Log(backgroundPanelOK.activeSelf);
    }

    public void ClickButtonNext()
    {
        action?.Invoke();
    }
}
