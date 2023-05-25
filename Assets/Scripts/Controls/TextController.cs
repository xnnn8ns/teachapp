using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private UniWebView webView;

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
        ////StartCoroutine(StartInitTextType());
        //AddTestText();
        //GetFromJSON();
        //countTheories = _theories.Count;
        //countText = _theories[indexCurrentTheory].GetTextList().Count;
        //indexCurrentTheory = -1;// cause in next code will be increase by 1
        //NextTheory();
        ShowWebView();
        //InitTextType();
    }

    private void InitTextType()
    {
        //try
        //{
            GameObject newTextItem = Instantiate(textItemPrefab, scrollParent);
            TextAnimation textCurrentForType = newTextItem.GetComponentInChildren<TextAnimation>();
            string strFull = _theories[indexCurrentTheory].GetTextList()[indexCurrentText].ToString();
            //textCurrentForType.StartType(strFull, NextType);
            textCurrentForType.GetComponentInChildren<Text>().text = strFull;
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

    private void AddTestText()
    {
        string testText = "This is some <b><size=50><color=#ff0000ff>Text</color></size></b>";
        Theory theory = new Theory();
        theory.Title = "Test";
        List<string> texts = new List<string>();
        texts.Add(testText);

        theory.SetTextList(texts);
        _theories.Add(theory);
    }

    private void ShowWebView()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("HTML_Theory/th_1");
        string txt = mytxtData.text;
        webView.gameObject.SetActive(true);

        webView.Frame = new Rect(0, 0, Screen.width, Screen.height*0.9f);

        //// Make the web view center in the screen with size 500x500:
        //var side = 500;
        //var x = (Screen.width - side) / 2.0f;
        //var y = (Screen.height - side) / 2.0f;
        //webView.Frame = new Rect(x, y, side, side);
        //webView.Load("https://vz.ru/");
        webView.LoadHTMLString(txt, "", false);
    }

    public void ClickReturnFromTheory()
    {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }
}
