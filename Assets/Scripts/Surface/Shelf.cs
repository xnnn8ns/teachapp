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

    public event Action<bool, Shelf> ClickShelf;

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

    private void SetAnswersOnShelf()
    {
        float widthRaw = 0f;
        SetTestShelfChecker(false);

        foreach (var item in _questionsShelved)
        {
            Vector3 startPoint = _shelfArea.position;
            startPoint.x -= _shelfArea.localScale.x / 2;

            startPoint.x += widthRaw;

            Vector3 offsetPoint = Vector3.zero;
            offsetPoint.x += item.transform.localScale.x / 2;

            Vector3 targetPoint = startPoint + offsetPoint;
            item.transform.position = targetPoint;

            widthRaw += item.transform.localScale.x + 0.1f;
        }
    }

    public void RemoveAnswerFromShelf(AnswerSurface transformChild)
    {   
        _questionsShelved?.Remove(transformChild);
        SetAnswersOnShelf();
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
            if(_questionsShelved[i].Answer.IsRightInputValuesForShelf(shelfIndex, i))
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
