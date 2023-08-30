using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Shelf : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Transform _shelfArea;
    [SerializeField]
    private  GameObject _shelfChecker;

    public bool IsRawAnswersShelf = false;
    public bool IsEnabled = true;
    public event Action<bool, Shelf> ClickShelf;
    private bool _isShelfFirstCompleted = false;
    public List<AnswerSurface> _questionsShelved = new List<AnswerSurface>();


    public void AddAnswerToShelfByDrag(AnswerSurface transformChild, bool isToEnd = false)
    {
        float currentX = transformChild.transform.position.x;
        int currentIndex = 0;
        foreach (var item in _questionsShelved)
        {
            if (item.transform.position.x > currentX)
                break;
            currentIndex++;
        }
        if(isToEnd)
            _questionsShelved.Add(transformChild);
        else
            _questionsShelved.Insert(currentIndex, transformChild);

        SetAnswersOnShelf();
    }

    public void AddAnswerToShelfOnRightPlace(List<AnswerSurface> transformList)
    {
        _questionsShelved = transformList;

        SetAnswersOnShelf();
    }

    public void AddAnswerToShelf(AnswerSurface transformChild)
    {
        Debug.Log("AddAnswerToShelf");
        _questionsShelved.Add(transformChild);
        
        SetAnswersOnShelf();
    }

    public void SetFirstAnswerForRawShelfCompleted()
    {
        _isShelfFirstCompleted = true;
    }

    private void SetAnswersOnShelf()
    {
        //if (!IsRawAnswersShelf || !_isShelfFirstCompleted)
            SetAnswersOnAnswerShelf();
        //else
        //    SetAnswersByBasePositoinShelf();
    }

    private void SetAnswersByBasePositoinShelf()
    {
        foreach (var item in _questionsShelved)
            item.transform.DOMove(item.BasePosition, 0.25f);
        //item.transform.position = item.BasePosition;
    }

    private void SetAnswersOnAnswerShelf2()
    {
        float widthRaw = 0f;
        SetTestShelfChecker(false);

        int countRows = 0;

        foreach (var item in _questionsShelved)
        {
            Vector3 startPoint = _shelfArea.position;

            startPoint.x += 0.075f;


            startPoint.x -= _shelfArea.localScale.x / 2;

            startPoint.x += widthRaw;

            widthRaw += item.transform.localScale.x + 0.05f;

            

            float yPosition = startPoint.y - 0.55f * countRows + _shelfArea.transform.localScale.y / 2 - 0.28f;

            startPoint.y = yPosition;

            Vector3 offsetPoint = Vector3.zero;
            offsetPoint.x += item.transform.localScale.x / 2;

            Vector3 targetPoint = startPoint + offsetPoint;
            //item.transform.position = targetPoint;
            item.transform.DOMove(targetPoint, 0.25f);

            if (widthRaw > 4f)
            {
                countRows++;
                widthRaw = 0;
            }

            if (IsRawAnswersShelf)
                item.BasePosition = targetPoint;
        }
    }

    private void SetAnswersOnAnswerShelf()
    {
        float widthRaw = 0f;
        SetTestShelfChecker(false);

        int countRows = 0;

        foreach (var item in _questionsShelved)
        {
            Vector3 startPoint = _shelfArea.position;

            startPoint.x += 0.05f;


            startPoint.x -= _shelfArea.localScale.x / 2;

            startPoint.x += widthRaw;

            widthRaw += item.transform.localScale.x + 0.05f;

            if (widthRaw > 4.9f)
            {
                countRows++;
                widthRaw = 0;
                startPoint = _shelfArea.position;
                startPoint.x += 0.05f;
                startPoint.x -= _shelfArea.localScale.x / 2;
                startPoint.x += widthRaw;
                widthRaw += item.transform.localScale.x + 0.05f;
            }

            float yPosition = startPoint.y - 0.55f * countRows + _shelfArea.transform.localScale.y / 2 - 0.28f;

            startPoint.y = yPosition;

            Vector3 offsetPoint = Vector3.zero;
            offsetPoint.x += item.transform.localScale.x / 2;

            Vector3 targetPoint = startPoint + offsetPoint;
            //item.transform.position = targetPoint;
            item.transform.DOMove(targetPoint, 0.25f);

            

            if (IsRawAnswersShelf)
                item.BasePosition = targetPoint;
        }
    }

    public void RemoveAnswerFromShelf(AnswerSurface transformChild)
    {   
        _questionsShelved?.Remove(transformChild);
        if (!IsRawAnswersShelf) {
            SetAnswersOnShelf();
        }
    }

    public bool IsAnswerInsideShelfBorders(AnswerSurface answerForCheck)
    {
        if (Mathf.Abs(answerForCheck.transform.position.y - _shelfArea.position.y) <= 0.3f)
            return true;
        else
            return false;
    }

    public List<AnswerSurface> GetAnswerList()
    {
        return _questionsShelved;
    }

    public bool IsRightAnswersInShelf(Question question, int shelfIndex)
    {
        //Debug.Log(_questionsShelved.Count);
        if (_questionsShelved.Count != question.GetCountRigthAnswersForRowIndex(shelfIndex))
            return false;

        for (int i = 0; i < _questionsShelved.Count; i++)
        {
            if(_questionsShelved[i].GetAnswer().IsRightInputValuesForShelf(shelfIndex, i))
                continue;
            else
                return false;
        }
        return true;
    }

    public void DestroyAllObjectsOnShelf()
    {
        for (int i = _questionsShelved.Count - 1; i >= 0; i--)
        {
            Destroy(_questionsShelved[i].gameObject);
        }
        _questionsShelved.Clear();
    }

    public void SetTestShelfChecker(bool value)
    {
        _shelfChecker.SetActive(value);
    }

    public bool GetTestShelfChecker()
    {
        return _shelfChecker.activeSelf;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Settings.IsTurnOnClickPointerListener)
            return;

        Debug.Log("OnPointerClick");
        SetTestShelfChecker(!_shelfChecker.activeSelf);
        ClickShelf.Invoke(_shelfChecker.activeSelf, this);
    }

    public bool GetIsShelfFull(AnswerSurface additionalAnswerSurface)
    {
        float widthRaw = 0f;
        foreach (var item in _questionsShelved)
        {
            Vector3 startPoint = _shelfArea.position;

            startPoint.x += 0.075f;


            startPoint.x -= _shelfArea.localScale.x / 2;

            startPoint.x += widthRaw;

            widthRaw += item.transform.localScale.x + 0.05f;

            if (widthRaw + additionalAnswerSurface.transform.localScale.x > 4.9f)
            {
                return true;
            }
        }
        return false;
    }
}
