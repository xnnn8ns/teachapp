using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    private Vector2 _firstTouchPosition, _holdTouchPosition, _startDragTouchPosition, _currentDragTouchPosition;
    private Vector2 _direction = Vector2.zero;
    private static float LastUserActionTime = 0;

    public event Action<Vector2> StartTouchArise;
    public event Action<Vector2> StopTouchArise;
    public event Action<Vector2> HoldTouchArise;

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                StartTouch();
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended ||
                           Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                StopTouch();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved
                        || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                HoldTouch();
            }
        }
    }

    private void StartTouch()
    {
        var touchPosition = Input.GetTouch(0).position;
        _firstTouchPosition = _holdTouchPosition = touchPosition;
        StartTouchArise?.Invoke(touchPosition);
        LastUserActionTime = Time.timeSinceLevelLoad;

    }

    private void OnMouseDown()
    {
        var touchPosition = Input.mousePosition;
        _firstTouchPosition = _holdTouchPosition = touchPosition;
        StartTouchArise?.Invoke(touchPosition);
        LastUserActionTime = Time.timeSinceLevelLoad;

    }

    private void StopTouch()
    {
        StopTouchArise?.Invoke(Input.GetTouch(0).position);
    }

    private void OnMouseUp()
    {
        StopTouchArise?.Invoke(Input.mousePosition);
    }

    private void HoldTouch()
    {
        _holdTouchPosition = Input.GetTouch(0).position;
        var touchOffset = Vector2.ClampMagnitude(_holdTouchPosition - _firstTouchPosition, 700f);

        if (touchOffset != Vector2.zero)
            _direction = touchOffset;

        _firstTouchPosition = _holdTouchPosition;
        HoldTouchArise?.Invoke(_holdTouchPosition);
        LastUserActionTime = Time.timeSinceLevelLoad;
    }

    private void OnMouseDrag()
    {
        _holdTouchPosition = Input.mousePosition;
        var touchOffset = Vector2.ClampMagnitude(_holdTouchPosition - _firstTouchPosition, 700f);

        if (touchOffset != Vector2.zero)
            _direction = touchOffset;

        _firstTouchPosition = _holdTouchPosition;
        HoldTouchArise?.Invoke(_holdTouchPosition);
        LastUserActionTime = Time.timeSinceLevelLoad;
    }
}
