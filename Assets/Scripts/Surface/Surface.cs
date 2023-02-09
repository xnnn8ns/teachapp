using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Surface : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshPro _titleText;

    private int _index = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(_index);
        GetComponent<AnimationExecuter>()?.StartLeftRightTurn();
    }

    public void SetTitle(string title, int index=0)
    {
        _titleText.text = title;
        _index = index;
    }
}
