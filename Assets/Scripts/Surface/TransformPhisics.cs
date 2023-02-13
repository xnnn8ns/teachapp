using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPhisics : MonoBehaviour
{
    private TouchDetector _touchDetector;
    private Vector3 _offsetOnStartDragging;
    private bool isStartTouch = false;
    private const float MULTY_DRAG_COEF = 8f;

    private void Drag(Vector2 vector)
    {
        if (!isStartTouch)
            return;

        vector *= MULTY_DRAG_COEF;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(vector.x, vector.y, Camera.main.nearClipPlane));
        Vector3 newPosition = point + _offsetOnStartDragging;
        transform.localPosition = newPosition;
    }

    private void StartDrag(Vector2 vector)
    {
        //if (!IsTouchedCard(vector)
        //    ||
        //    (GlobalState.CARD_SELECTED && !_isSelected))
        //    return;

        //if (_isSelected)
        //    StartCoroutine(StartUnSelected());
        //else
        //    StartCoroutine(StartAppearSelected());
    }

    private void StopDrag(Vector2 vector)
    {
        if (!isStartTouch)
            return;
        isStartTouch = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ~TransformPhisics()
    {
        if (_touchDetector == null)
            return;

        _touchDetector.StartTouchArise -= StartDrag;
        _touchDetector.StopTouchArise -= StopDrag;
        _touchDetector.HoldTouchArise -= Drag;
    }
}
