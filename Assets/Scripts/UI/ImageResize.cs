using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageResize : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private float _partOfScreenHeight;

    [SerializeField]
    private float _coefProportion;

    // Start is called before the first frame update
    void Start()
    {
        SetProperSize();
    }

    private void SetProperSize()
    {
        RectTransform rect = _image.GetComponent<RectTransform>();
        float targetHeight = Screen.height * _partOfScreenHeight;
        float targetWidth = targetHeight / _coefProportion;
        //Debug.Log((float)Screen.height/Screen.width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
    }

}
