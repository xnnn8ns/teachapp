using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    [SerializeField]
    private Text _textType;

    [SerializeField]
    private Button _clickFinishButton;

    private string _textCash;

    private Coroutine _coroutineType = null;
    private Action _resultCurrentType = null;

    void Awake()
    {
        //_textCash = _textType.text;
        _textType.text = "";
        //StartCoroutine(TypeText());
    }

    public void StartType(string text, Action result)
    {
        _textCash = text;
        _resultCurrentType = result;
        _textType.text = "";
        _coroutineType = StartCoroutine(TypeText());
    }

    public void ShowFullText(string text, Action result)
    {
        _textCash = text;
        _resultCurrentType = result;
        _textType.text = text;
        //_coroutineType = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        _textType.text = "<color=#ffffff>" + _textCash + "</color>";
        yield return new WaitForSeconds(0.1f);
        string str = "";
        int countStr = 0;
        foreach (char c in _textCash)
        {
            str += c;
            string colorStr = "<color=#000000>" + str + "</color>";
            string colorReminderStr = "";
            if (str.Length < _textCash.Length)
            {
                string reminderStr = _textCash.Substring(countStr+1);
                colorReminderStr = "<color=#ffffff>" + reminderStr + "</color>";
            }
            _textType.text = colorStr + colorReminderStr;
            countStr++;
            yield return new WaitForSeconds(0.0725f);
        }
        _resultCurrentType?.Invoke();
        _clickFinishButton?.gameObject.SetActive(false);
    }

    public void ClickButtonFinishReadingByUser()
    {
        try
        {
            if (_coroutineType != null)
                StopCoroutine(_coroutineType);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        _textType.text = _textCash;
        _resultCurrentType?.Invoke();
        _clickFinishButton?.gameObject?.SetActive(false);
    }
}
