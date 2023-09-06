using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeaderTheory : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _theoryTitle;
    [SerializeField]
    private TMP_Text _theorySubTitle;

    private int _id = 0;
    Action<int> _actionCLick;

    public void ClickTheory()
    {
        _actionCLick?.Invoke(_id);
    }

    public void SetID(int id, Action<int> actionCLick)
    {
        _id = id;
        _actionCLick = actionCLick;
    }
}
