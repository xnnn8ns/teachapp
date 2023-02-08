using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerSurface : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _titleText;

    public void SetTitle(string title)
    {
        _titleText.text = title;
    }
}
