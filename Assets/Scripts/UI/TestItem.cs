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
        _textValue.text = testAnswer;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickTest.Invoke(TestIndex);
    }
}
