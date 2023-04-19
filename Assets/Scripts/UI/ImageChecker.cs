using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageChecker : MonoBehaviour
{
    [SerializeField]
    private ImageItem[] _imageItems;

    private Color _colorSelected = Color.green;
    private Color _colorUnSelected = Color.gray;
    private List<Answer> _answers = new List<Answer>();

    const string Path = "ImagesTest/";

    private Sprite GetImageByResourceName(string resourceName)
    {
        return Resources.Load<Sprite>(Path + resourceName);
    }

    public void SetImagesFromAnswers(List<Answer> answers)
    {
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

    public void SetSingleSelectedImage(int indexImageForSelect)
    {
        for (int i = 0; i < _answers.Count; i++)
        {
            if(i == indexImageForSelect)
                _imageItems[i].SetBackColor(_colorSelected);
            else
                _imageItems[i].SetBackColor(_colorUnSelected);

        }
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
