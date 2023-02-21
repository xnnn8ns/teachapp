using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Surface : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _titleText;

    public void SetTitle(string title)
    {
        _titleText.text = title;
    }

}
