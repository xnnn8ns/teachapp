using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theory :  Information
{
    private List<string> _textList = new List<string>();

    public void SetTextList(List<string> textList)
    {
        _textList = textList;
    }

    public List<string> GetTextList()
    {
        return _textList;
    }
}
