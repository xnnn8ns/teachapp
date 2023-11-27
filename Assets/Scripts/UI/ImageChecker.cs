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

    private Color _colorSelected = Color.yellow;
    //private Color _colorUnSelected = Color.HSVToRGB(0.96875f, 0.86328125f, 0.9765625f);
    private Color _colorUnSelected = Color.white;
    
    private List<Answer> _answers = new List<Answer>();
    private Action _actionCallBack;

    const string Path = "ImagesTest/";

    private void Awake()
    {
        Color color;
        bool result = ColorUtility.TryParseHtmlString("#F8DDFA", out color);
        Debug.Log(color.ToString());
        if (result && color != null)
        {
            _colorUnSelected = color;
        }
    }

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
        float maxSize = 0;
        for (int i = 0; i < _answers.Count; i++)
        {
            string title = _answers[i].Title;
            _testItems[i].TestIndex = i;
            _testItems[i].SetTestValue(title);
            _testItems[i].SetBackColor(_colorUnSelected);
            _testItems[i].ClickTest += SetSingleSelectedTest;
            if (maxSize < title.Length)
                maxSize = title.Length;
        }
        float maxFloatSize = Screen.height/30f;
        if (maxSize > 3)
            maxFloatSize = Screen.height / 48f;
        Debug.Log("Screen.height: " + Screen.height);
        for (int i = 0; i < _answers.Count; i++)
        {
            _testItems[i].SetTestFontSize(maxFloatSize);
        }
        //StartCoroutine(SetFontSize(minSize));
    }

    private IEnumerator SetFontSize(float fontSize)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < _answers.Count; i++)
        {
            _testItems[i].SetTestFontSize(fontSize);
        }
        yield break;
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
