using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageItem : MonoBehaviour, IPointerClickHandler
{

    private Image _imageBack;
    private Image _imageValue;
    public int ImageIndex = 0;
    public event Action<int> ClickImage;

    private void Awake()
    {
        _imageBack = GetComponent<Image>();
        _imageValue = transform.GetChild(0)?.GetComponent<Image>();
    }

    public void SetBackColor(Color color)
    {
        _imageBack.color = color;
    }

    public Color GetBackColor()
    {
        return _imageBack.color;
    }

    public void SetImage(Sprite sprite)
    {
        _imageValue.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickImage.Invoke(ImageIndex);
    }
}
