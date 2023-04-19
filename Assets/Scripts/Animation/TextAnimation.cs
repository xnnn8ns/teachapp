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

    private IEnumerator TypeText()
    {
        foreach (char c in _textCash)
        {
            _textType.text += c;
            yield return new WaitForSeconds(0.0725f);
        }
        _resultCurrentType?.Invoke();
        _clickFinishButton?.gameObject.SetActive(false);
    }

    public void ClickButtonFinishReadindByUser()
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
        _clickFinishButton.gameObject.SetActive(false);
    }
}
