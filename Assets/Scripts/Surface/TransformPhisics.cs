using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPhisics : MonoBehaviour
{
    [SerializeField]
    private TouchDetector _touchDetector;
    [SerializeField]
    private QuestionInitializer _questionInitializer;
    private Vector3 _offsetOnStartDragging;
    private Vector3 _startDraggingPosition;
    private float timeStart = 0f;
    private bool _isStartTouch = false;
    private Transform _draggingTransform;
    private readonly Vector3 _offsetDraggingZ = new Vector3(0f,0f,0.15f);
    private const float _distanceFromCameraToQuestionPlane = 10f;

    private void Drag(Vector2 vector)
    {
        if (!_isStartTouch || _draggingTransform == null || !Settings.IsNeedDragDropTouchDetector)
            return;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(vector.x, vector.y, _distanceFromCameraToQuestionPlane));
        point -= _offsetDraggingZ;
        _draggingTransform.position = point - _offsetOnStartDragging;
    }

    private void StartDrag(Vector2 vector)
    {
        if (_isStartTouch || !Settings.IsNeedDragDropTouchDetector)
            return;
        Vector3 pointTouch = GetWorldPointFromScreenTouch(vector);
        _draggingTransform = IsTouchDownAnswerSurface(pointTouch);
        if (_draggingTransform != null)
        {
            _offsetOnStartDragging = pointTouch - _draggingTransform.position;
            _startDraggingPosition = _draggingTransform.position;
            _isStartTouch = true;
            _questionInitializer.RemoveFromShelf(_draggingTransform);
            timeStart = Time.time;
        }
    }

    private void StopDrag(Vector2 vector)
    {
        if (!_isStartTouch || _draggingTransform == null || !Settings.IsNeedDragDropTouchDetector)
            return;
        _isStartTouch = false;
        float dist = Vector3.Distance(_draggingTransform.position, _startDraggingPosition);
        //Debug.Log(dist);
        _draggingTransform.position += _offsetDraggingZ;

        bool isClick = false;
        if (dist < 0.2f && timeStart + 0.25f > Time.time)
            isClick = true;
        Debug.Log(isClick);
        _questionInitializer.CheckAnswerAfterDrop(_draggingTransform, isClick);
        _draggingTransform = null;
    }

    private void Start()
    {
        SignListeners();
    }

    private void SignListeners()
    {
        _touchDetector.StartTouchArise += StartDrag;
        _touchDetector.StopTouchArise += StopDrag;
        _touchDetector.HoldTouchArise += Drag;
    }

    private void UnSignListeners()
    {
        if (_touchDetector == null)
            return;

        _touchDetector.StartTouchArise -= StartDrag;
        _touchDetector.StopTouchArise -= StopDrag;
        _touchDetector.HoldTouchArise -= Drag;
    }

    ~TransformPhisics()
    {
        UnSignListeners();
    }

    private Transform IsTouchDownAnswerSurface(Vector3 pointTouch)
    {
        List<GameObject> _answers = _questionInitializer.GetAnswersList();
        List<GameObject> _answersInTouchArea = new List<GameObject>();
        if (_answers == null || _answers.Count == 0)
            return null;

        foreach (var item in _answers)
        {
            float widthHalf = item.transform.localScale.x / 2;
            float heightHalf = item.transform.localScale.y / 2;
            if (item.transform.position.y + heightHalf > pointTouch.y
                &&
                item.transform.position.y - heightHalf < pointTouch.y
                &&
                item.transform.position.x + widthHalf > pointTouch.x
                &&
                item.transform.position.x - widthHalf < pointTouch.x)
            {
                _answersInTouchArea.Add(item);
            }
        }
        if (_answersInTouchArea.Count == 0)
            return null;

        Transform closestToCameraTransform = _answersInTouchArea[0].transform;
        if (_answersInTouchArea.Count > 1) {
            float distanceMin = Vector3.Distance(Camera.main.transform.position, closestToCameraTransform.position);
            for (int i = 1; i < _answersInTouchArea.Count; i++)
            {
                float distanceCurrent = Vector3.Distance(Camera.main.transform.position, _answersInTouchArea[i].transform.position);
                if (distanceCurrent < distanceMin)
                {
                    distanceMin = distanceCurrent;
                    closestToCameraTransform = _answersInTouchArea[i].transform;
                }
            }
        }

        return closestToCameraTransform;
    }

    private Vector3 GetWorldPointFromScreenTouch(Vector2 vectorTouch)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(vectorTouch.x, vectorTouch.y, _distanceFromCameraToQuestionPlane));
    }
}
