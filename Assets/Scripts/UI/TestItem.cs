using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TestItem : MonoBehaviour, IPointerClickHandler
{

    private Image _imageBack;
    private TextMeshProUGUI _textValue;
    public int TestIndex = 0;
    public event Action<int> ClickTest;

    private void Awake()
    {
        _imageBack = GetComponent<Image>();
        _textValue = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetBackColor(Color color)
    {
        _imageBack.color = color;
    }

    public Color GetBackColor()
    {
        return _imageBack.color;
    }

    public void SetTestValue(string testAnswer)
    {
        //_textValue.enableAutoSizing = true;
        _textValue.text = testAnswer;
    }

    public float GetFontSize()
    {
        Debug.Log("_textValue.fontSizeMax: " + _textValue.fontSizeMax);
        return _textValue.fontSizeMax;
    }

    public void SetTestFontSize(float value)
    {
        //_textValue.enableAutoSizing = false;
        //Debug.Log("_textValue.fontSizeMax: " + _textValue.fontSizeMax);
        _textValue.fontSizeMax = value;
        _textValue.fontSizeMin = Screen.height / 58f;
        //Debug.Log("_textValue.fontSizeMax2: " + value);
        //Debug.Log("_textValue.fontSizeMax3: " + _textValue.fontSizeMax);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickTest.Invoke(TestIndex);
    }
}
