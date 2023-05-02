using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField]
    private GameObject textItemPrefab;
    [SerializeField]
    private GameObject buttonFinishPrefab;
    [SerializeField]
    private GameObject headerPrefab;

    [SerializeField]
    private Transform scrollParent;

    private List<Theory> _theories = new List<Theory>();
    private int indexCurrentTheory = 0;
    private int indexCurrentText = 0;
    private int countText = 0; //3;
    private int countTheories = 0; //3;
    //private static bool IsTyping = false;

    private void Start()
    {
        //StartCoroutine(StartInitTextType());
        GetFromJSON();
        countTheories = _theories.Count;
        countText = _theories[indexCurrentTheory].GetTextList().Count;
        indexCurrentTheory = -1;// cause in next code will be increase by 1
        NextTheory();
        //InitTextType();
    }

    private void InitTextType()
    {
        //try
        //{
            GameObject newTextItem = Instantiate(textItemPrefab, scrollParent);
            TextAnimation textCurrentForType = newTextItem.GetComponentInChildren<TextAnimation>();
            string strFull = _theories[indexCurrentTheory].GetTextList()[indexCurrentText].ToString();
            textCurrentForType.StartType(strFull, NextType);
        //}
        //catch (Exception ex)
        //{
        //    Debug.Log(ex.Message);
        //}
        
    }

    private void InitButtonFinish()
    {
        GameObject newButtonFinish = Instantiate(buttonFinishPrefab, scrollParent);
        TheoryTypeFinish buttonFinish = newButtonFinish.GetComponentInChildren<TheoryTypeFinish>();
        buttonFinish.SetCallBack(NextTheory);
    }

    private void InitHeader(string headerValue)
    {
        GameObject newHeader = Instantiate(headerPrefab, scrollParent);
        Text buttonFinish = newHeader.GetComponentInChildren<Text>();
        buttonFinish.text = headerValue;
    }

    private void InitTextTypeTest()
    {
        GameObject newTextItem = Instantiate(textItemPrefab, scrollParent);
        TextAnimation _textCurrentForType = newTextItem.GetComponentInChildren<TextAnimation>();
        string strType = "Some text here " + indexCurrentText.ToString();
        string strFull = "";
        for (int i = 0; i < 3; i++)
            strFull += strType + ", ";
        
        _textCurrentForType.StartType(strFull, NextType);
    }

    //private IEnumerator StartInitTextType()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    while (IsTyping)
    //    {
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //    yield break;
    //}

    private void NextType()
    {
        indexCurrentText++;
        if (indexCurrentText < countText)
            InitTextType();
        else
            InitButtonFinish();
    }

    private void NextTheory()
    {
        indexCurrentTheory++;
        indexCurrentText = 0;

        if (indexCurrentTheory < countTheories)
        {
            countText = _theories[indexCurrentTheory].GetTextList().Count;
            InitHeader(_theories[indexCurrentTheory].Title);
            InitTextType();
        }
        Debug.Log("Next Theory");
    }

    private void GetFromJSON()
    {
        string strJSON = "";
        strJSON = Resources.Load<TextAsset>("TA_data_theory").text;
        RawDataTheory treoryFromJSON = null;
        try
        {
            treoryFromJSON = JsonConvert.DeserializeObject<RawDataTheory>(strJSON, Settings.JsonSettings);
            foreach (var item in treoryFromJSON.RawTheories)
            {
                Theory theory = new Theory();
                theory.Title = item.Title;
                List<string> texts = new List<string>();
                foreach (var itemSub in item.RawTexts)
                    texts.Add(itemSub);
                
                theory.SetTextList(texts);
                _theories.Add(theory);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
