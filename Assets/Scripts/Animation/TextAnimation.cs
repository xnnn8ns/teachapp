using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    [SerializeField]
    private Text _textType;

    private string _textCash;

    void Awake()
    {
        _textCash = _textType.text;
        _textType.text = "";
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        foreach (char c in _textCash)
        {
            _textType.text += c;
            yield return new WaitForSeconds(0.0725f);
        }
    }
}
