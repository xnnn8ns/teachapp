using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private Text _textButtonCheckHeader;

    private void Start()
    {
        SetHeaders();
    }

    public void SetLevelProgress(float shareOfCompletedBar)
    {
        _slider.value = shareOfCompletedBar;
    }

    private void SetHeaders()
    {
        _textButtonCheckHeader.text = LangAsset.GetValueByKey("Check");
    }
}
