using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheoryTypeFinish : MonoBehaviour
{

    private Action _resultFinish = null;

    public void SetCallBack(Action result)
    {
        _resultFinish = result;
    }

    public void ClickButtonFinishReadindByUser()
    {
        _resultFinish?.Invoke();
    }
}
