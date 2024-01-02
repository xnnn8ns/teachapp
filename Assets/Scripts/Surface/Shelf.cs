using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;
using TMPro;

public class Shelf : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Transform _shelfArea;
    [SerializeField]
    private  GameObject _shelfChecker;
    [SerializeField]
    private Material _materialDefault;
    [SerializeField]
    private Material _materialCompleted;
    [SerializeField]
    private Material _materialWrong;
    [SerializeField]
    private Material _materialFull;
    [SerializeField]
    private TextMeshPro _textHint;

    private bool _isRawAnswersShelf = false;
    private int _countAnswersToBeOnShelf = 0;
    public bool IsEnabled = true;
    public event Action<bool, Shelf> ClickShelf;
    private bool _isShelfFirstCompleted = false;
    public List<AnswerSurface> _questionsShelved = new List<AnswerSurface>();

    public void SetAsRawShelf(bool isVisible = true)
    {
        _shelfArea.GetComponent<Renderer>().material = _materialFull;
        _isRawAnswersShelf = true;
        GetComponent<Renderer>().enabled = isVisible;
    }

    public void AddAnswerToShelfByDrag(AnswerSurface transformChild, bool isToEnd = false)
    {
        float currentX = transformChild.transform.position.x;
        //if (IsRawAnswersShelf)
        //    currentX = transformChild.BasePosition.x;
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
        //Debug.Log("AddAnswerToShelf");
        _questionsShelved.Add(transformChild);
        
        SetAnswersOnShelf();
    }

    public void SetFirstAnswerForRawShelfCompleted()
    {
        _isShelfFirstCompleted = true;
    }

    private void SetAnswersOnShelf()
    {
        SetHint();
        if (!_isRawAnswersShelf || !_isShelfFirstCompleted)
            SetAnswersOnAnswerShelf();
        else
            SetAnswersByBasePositoinShelf();
    }

    private void SetAnswersByBasePositoinShelf()
    {
        foreach (var item in _questionsShelved)
            item.transform.DOMove(item.BasePosition, 0.25f);
        //item.transform.position = item.BasePosition;
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

            if (widthRaw > 4.9f && _isRawAnswersShelf) // for auto set on next line if this full
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
            targetPoint.z = -0.02f;
            //item.transform.position = targetPoint;
            item.transform.DOMove(targetPoint, 0.25f);

            

            if (_isRawAnswersShelf)
                item.BasePosition = targetPoint;
        }
    }

    public void RemoveAnswerFromShelf(AnswerSurface transformChild)
    {   
        _questionsShelved?.Remove(transformChild);
        if (!_isRawAnswersShelf) {
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

    //public bool IsRightAnswersInShelf(Question question, int shelfIndex)
    //{
    //    //Debug.Log(_questionsShelved.Count);
    //    if (_questionsShelved.Count != question.GetCountRigthAnswersForRowIndex(shelfIndex))
    //        return false;

    //    for (int i = 0; i < _questionsShelved.Count; i++)
    //    {
    //        if(_questionsShelved[i].GetAnswer().IsRightInputValuesForShelf(shelfIndex, i))
    //            continue;
    //        else
    //            return false;
    //    }
    //    return true;
    //}

    public bool IsRightAnswersInShelf2(Question question, int shelfIndex)
    {
        //Debug.Log(_questionsShelved.Count);
        if (_questionsShelved.Count != question.GetCountRigthAnswersForRowIndex(shelfIndex))
            return false;

        for (int i = 0; i < _questionsShelved.Count; i++)
        {
            string answerCurrent = _questionsShelved[i].GetAnswer().Title;
            List<Answer> answerList = question.GetRigthAnswerList();
            foreach (var item in answerList)
            {
                if (item.PositionRowIndex == shelfIndex
                    &&
                    item.PositionCellIndex == i)
                {
                    if (item.Title != answerCurrent)
                        return false;
                    else
                        break;
                }
            }
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
        return false;
        //float widthRaw = 0f;
        //foreach (var item in _questionsShelved)
        //{
        //    Vector3 startPoint = _shelfArea.position;

        //    startPoint.x += 0.075f;


        //    startPoint.x -= _shelfArea.localScale.x / 2;

        //    startPoint.x += widthRaw;

        //    widthRaw += item.transform.localScale.x + 0.05f;

        //    if (widthRaw + additionalAnswerSurface.transform.localScale.x > 4.9f)
        //    {
        //        return true;
        //    }
        //}
        //return false;
    }

    public void SetCompleted()
    {
        _shelfArea.GetComponent<Renderer>().material = _materialCompleted;
        IsEnabled = false;
        foreach (var item in _questionsShelved)
            item.GetAnswer().IsEnabled = false;
        //foreach (var item in _questionsShelved)
        //    item.GetAnswer().IsOpenOnStart = true;
        Debug.Log("SetCompleted");
    }

    public void SetWrongCompleted()
    {
        _shelfArea.GetComponent<Renderer>().material = _materialWrong;
        IsEnabled = false;
        foreach (var item in _questionsShelved)
            item.GetAnswer().IsEnabled = false;
        //foreach (var item in _questionsShelved)
        //    item.GetAnswer().IsOpenOnStart = true;
        Debug.Log("SetWrongCompleted");
    }

    public void SetBackWrongCompleted()
    {
        foreach (var item in _questionsShelved)
        {
            if(!item.GetAnswer().IsOpenOnStart)
                item.GetAnswer().IsEnabled = true;
        }
        Debug.Log("SetBackWrongCompleted");
    }

    public void ReBuildBasePosition()
    {
        SetAnswersOnAnswerShelf();
    }

    public void SetHint()
    {
        if (IsEnabled && _questionsShelved.Count == 0)
        {
            _textHint.gameObject.SetActive(true);
            ScaleTextHint();
        }
        else
            _textHint.gameObject.SetActive(false);
        //string str = LangAsset.GetValueByKey("Blocks") + LangAsset.GetValueByKey(_countAnswersToBeOnShelf.ToString());

        _textHint.text = _countAnswersToBeOnShelf.ToString();
        //_textHint.text = "Blocks" + ": " + _countAnswersToBeOnShelf.ToString();
    }

    private void ScaleTextHint()
    {
        Vector3 scaleText = _textHint.transform.localScale;
        scaleText.x = transform.lossyScale.y;
        _textHint.transform.localScale = scaleText;
    }

    public void SetCountAnswerToBeOnShelf(int value)
    {
        _countAnswersToBeOnShelf = value;
    }
        
}