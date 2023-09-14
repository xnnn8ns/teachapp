using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageChecker : MonoBehaviour
{
    [SerializeField]
    private ImageItem[] _imageItems;
    [SerializeField]
    private TestItem[] _testItems;

    private Color _colorSelected = Color.green;
    private Color _colorUnSelected = Color.gray;
    private List<Answer> _answers = new List<Answer>();
    private Action _actionCallBack;

    const string Path = "ImagesTest/";

    private Sprite GetImageByResourceName(string resourceName)
    {
        return Resources.Load<Sprite>(Path + resourceName);
    }

    public void SetImagesFromAnswers(List<Answer> answers, Action action)
    {
        _actionCallBack = action;
        _answers = answers;
        for (int i = 0; i < _answers.Count; i++)
        {
            string title = _answers[i].Title;
            Sprite sprite = GetImageByResourceName(title);
            _imageItems[i].ImageIndex = i;
            _imageItems[i].SetImage(sprite);
            _imageItems[i].SetBackColor(_colorUnSelected);
            _imageItems[i].ClickImage += SetSingleSelectedImage;
        }
    }

    public void SetTestFromAnswers(List<Answer> answers, Action action)
    {
        _actionCallBack = action;
        _answers = answers;
        for (int i = 0; i < _answers.Count; i++)
        {
            string title = _answers[i].Title;
            _testItems[i].TestIndex = i;
            _testItems[i].SetTestValue(title);
            _testItems[i].SetBackColor(_colorUnSelected);
            _testItems[i].ClickTest += SetSingleSelectedTest;
        }
    }

    private void SetSingleSelectedTest(int indexTestForSelect)
    {
        for (int i = 0; i < _answers.Count; i++)
        {
            if (i == indexTestForSelect)
                _testItems[i].SetBackColor(_colorSelected);
            else
                _testItems[i].SetBackColor(_colorUnSelected);

        }
        _actionCallBack?.Invoke();
    }

    private void SetSingleSelectedImage(int indexImageForSelect)
    {
        for (int i = 0; i < _answers.Count; i++)
        {
            if(i == indexImageForSelect)
                _imageItems[i].SetBackColor(_colorSelected);
            else
                _imageItems[i].SetBackColor(_colorUnSelected);

        }
        _actionCallBack?.Invoke();
    }

    public void SetSelectedImage(int indexImageForSelect)
    {
        for (int i = 0; i < _answers.Count; i++)
        {
            if (i == indexImageForSelect)
                _imageItems[i].SetBackColor(_colorSelected);
        }
    }

    public bool GetIsRight()
    {
        for (int i = 0; i < _answers.Count; i++)
        {
            if (_imageItems[i].GetBackColor().Equals(_colorSelected) != _answers[i].IsRight)
                return false;
        }
        return true;
    }
}
