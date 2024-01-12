using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Avatar : MonoBehaviour, IPointerClickHandler
{
    public int AvatarID = 0;
    private Action<int> _action;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click");
        //Debug.Log(_action);
        _action?.Invoke(AvatarID);
    }

    public void SetID(int id, Action<int> action)
    {
        AvatarID = id;
        _action = action;
    }
}
