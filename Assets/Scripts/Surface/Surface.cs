using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Surface : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshPro _titleText;

    private Information _parentObject;
    private Transform _parentTransform;

    private Action<Information, Transform> _actionClickSurface;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(_index);
        _actionClickSurface.Invoke(_parentObject, _parentTransform);
    }

    public void SetTitle(string title)
    {
        _titleText.text = title;
    }

    public void SetActionCallback(Information parentObject, Transform parentTransform, Action<Information, Transform> actionClickSurface)
    {
        _parentObject = parentObject;
        _parentTransform = parentTransform;
        _actionClickSurface = actionClickSurface;
    }

}
