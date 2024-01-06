using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _textButtonCheckHeader;
    [SerializeField]
    private TextMeshProUGUI _textButtonCheckHeaderDisabled;

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
        _textButtonCheckHeader.text = LangAsset.GetValueByKey("Continue");
        _textButtonCheckHeaderDisabled.text = LangAsset.GetValueByKey("Continue");
    }
}
