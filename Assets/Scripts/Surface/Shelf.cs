using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shelf : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Transform _shelfArea;
    [SerializeField]
    private  GameObject _shelfChecker;
    public bool IsRawAnswersShelf = false;
    public event Action<bool, Shelf> ClickShelf;
    private bool _isShelfFirstCompleted = false;
    private List<AnswerSurface> _questionsShelved = new List<AnswerSurface>();

    public void AddAnswerToShelfByDrag(AnswerSurface transformChild)
    {
        float currentX = transformChild.transform.position.x;
        int currentIndex = 0;
        foreach (var item in _questionsShelved)
        {
            if (item.transform.position.x > currentX)
                break;
            currentIndex++;
        }
        _questionsShelved.Insert(currentIndex, transformChild);

        SetAnswersOnShelf();
    }

    public void AddAnswerToShelf(AnswerSurface transformChild)
    {
        _questionsShelved.Add(transformChild);
        
        SetAnswersOnShelf();
    }

    public void SetFirstAnswerForRarShelfCompleted()
    {
        _isShelfFirstCompleted = true;
    }

    private void SetAnswersOnShelf()
    {
        if (!IsRawAnswersShelf || !_isShelfFirstCompleted)
            SetAnswersOnAnswerShelf();
        else
            SetAnswersByBasePositoinShelf();
    }

    private void SetAnswersByBasePositoinShelf()
    {
        foreach (var item in _questionsShelved)
            item.transform.position = item.BasePosition;
    }

    private void SetAnswersOnAnswerShelf()
    {
        float widthRaw = 0f;
        SetTestShelfChecker(false);

        int countRows = 0;

        foreach (var item in _questionsShelved)
        {
            Vector3 startPoint = _shelfArea.position;

            float yPosition = startPoint.y - 0.55f * countRows + _shelfArea.transform.localScale.y / 2 - 0.28f;

            startPoint.x -= _shelfArea.localScale.x / 2;

            startPoint.x += widthRaw;

            startPoint.y = yPosition;

            Vector3 offsetPoint = Vector3.zero;
            offsetPoint.x += item.transform.localScale.x / 2;

            Vector3 targetPoint = startPoint + offsetPoint;
            item.transform.position = targetPoint;

            widthRaw += item.transform.localScale.x + 0.05f;

            if (widthRaw > 4)
            {
                countRows++;
                widthRaw = 0;
            }

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
}
