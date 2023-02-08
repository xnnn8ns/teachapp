using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionSurface : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _titleText;

    public void SetTitle(string title)
    {
        _titleText.text = title;
    }

}
