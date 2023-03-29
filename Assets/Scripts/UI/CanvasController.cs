using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    public void SetLevelProgress(float shareOfCompletedBar)
    {
        _slider.value = shareOfCompletedBar;
    }
}
