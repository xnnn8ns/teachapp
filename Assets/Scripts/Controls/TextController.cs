using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ResponseTheoryJson;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    private const string apiUrl = "http://45.12.239.30:8000/teach/";

    [SerializeField]
    private TMP_Text theoryTitleText;
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

    //private List<Theory> _theories = new List<Theory>();
    private static List<TheoryJSONItem> _theories = new List<TheoryJSONItem>();
    private static List<string> _theoriesString = new List<string>();
    private int indexCurrentTheory = 0;
    private int indexCurrentText = 0;
    private int countText = 0; //3;
    //private int countTheories = 0; //3;
                                   //private static bool IsTyping = false;

    private void Start()
    {
        theoryTitleText.text = "Раздел " + (Settings.Current_Theme + 1).ToString();
        //StartCoroutine(GetDataFromAPI(Settings.Current_Theme+1));
        //ShowWebView();
        InitTextTypeTest();
        //StartCoroutine(GetDataFromAPIOld());
        //GetFromJSON();
    }

    private void InitTextType()
    {
        //try
        //{
            GameObject newTextItem = Instantiate(textItemPrefab, scrollParent);
            TextAnimation textCurrentForType = newTextItem.GetComponentInChildren<TextAnimation>();
            //string strFull = _theories[indexCurrentTheory].GetTextList()[indexCurrentText].ToString();
        string strFull = _theoriesString[indexCurrentTheory];

        textCurrentForType.ShowFullText(strFull, NextType);
            //textCurrentForType.GetComponentInChildren<Text>().text = strFull;
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
        //string strType = "Some text here " + indexCurrentText.ToString();
        string strType = Resources.Load<TextAsset>("HTML_Theory/th_" + (Settings.Current_Theme + 1).ToString()).text;
        //string testText = @"This is some <b><size=50><color=#ff0000ff>Text</color></size></b>";
        //string strNew = @strType;
        //string strType = _theoriesString[indexCurrentTheory];
        //Debug.Log(strNew);
        //string strFull = "";
        //for (int i = 0; i < 3; i++)
        //    strFull += strType + ", ";
        //_textType.text = strType;
        _textCurrentForType.ShowFullText(strType, NextType);
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
        
        if (indexCurrentText < countText)
            InitTextType();
        else
            InitButtonFinish();
        indexCurrentText++;
    }

    private void NextTheory()
    {
        indexCurrentTheory++;
        indexCurrentText = 0;

        if (indexCurrentTheory < _theoriesString.Count)
        {
            countText = _theoriesString.Count;
            //InitHeader(_theories[indexCurrentTheory].Title);
            InitTextType();
        }
        Debug.Log("Next Theory");
    }

    private void GetFromJSON()
    {
        string strJSON = "";
        strJSON = Resources.Load<TextAsset>("HTML_Theory/th_1").text;
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
                //_theories.Add(theory);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void AddTestText()
    {
        string testText = @"This is some <b><size=50><color=#ff0000ff>Text</color></size></b>";
        Theory theory = new Theory();
        theory.Title = "Test";
        List<string> texts = new List<string>();
        texts.Add(testText);

        theory.SetTextList(texts);
        //_theories.Add(theory);
    }

    private void ShowWebView(string content)
    {
        //TextAsset mytxtData = (TextAsset)Resources.Load("HTML_Theory/th_1");
        //string txt = mytxtData.text;
        webView.gameObject.SetActive(true);
        
        webView.OnShouldClose += (view) => OnShouldCloseWebView(view);
        //webView.Alpha = 1.0f;

        webView.Frame = new Rect(0, -Screen.height * 0.1f, Screen.width, Screen.height);
        //// Make the web view center in the screen with size 500x500:
        //var side = 500;
        //var x = (Screen.width - side) / 2.0f;
        //var y = (Screen.height - side) / 2.0f;
        //webView.Frame = new Rect(x, y, side, side);
        //webView.Load("https://vz.ru/");
        //webView.OnMessageReceived += (view, message) => RecieveMessage(view, message);
        webView.LoadHTMLString(content, "", false);
        webView.SetShowToolbar(
                false,  // Show or hide?         true  = show
                false, // With animation?       false = no animation
                false,  // Is it on top?         true  = top
                false  // Should adjust insets? true  = avoid overlapping to web view
            );
    }

    //private void RecieveMessage(UniWebView webView, UniWebViewMessage message)
    //{
    //    ClickReturnFromTheory();
    //}

    private bool OnShouldCloseWebView(UniWebView webView)
    {
        ClickReturnFromTheory();
        return true;
    }

    public void ClickReturnFromTheory()
    {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    private IEnumerator GetDataFromAPI(int theoryID)
    {
        string fileName = Settings.theoryFilePath + theoryID.ToString() + ".txt";
        string content = "";
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            content = File.ReadAllText(Application.persistentDataPath + fileName);
        }
        else
        {
            using (UnityWebRequest www = UnityWebRequest.Get(apiUrl + theoryID.ToString()))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    FileStream fs = File.Create(Application.persistentDataPath + fileName);
                    fs.Dispose();
                    TextAsset txt = (TextAsset)Resources.Load("th_" + theoryID.ToString(), typeof(TextAsset));
                    content = txt.text;

                }
                else
                {
                    FileStream fs = File.Create(Application.persistentDataPath + fileName);
                    fs.Dispose();
                    content = www.downloadHandler.text;
                    File.WriteAllText(Application.persistentDataPath + fileName, content);
                }
            }
        }
        //ShowWebView(content);
    }

    private IEnumerator GetDataFromAPIOld()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("HTTP Error: " + www.error);
            }
            else
            {
                Debug.Log("API Response: " + www.downloadHandler.text);
                TheoryJSON response = TheoryJSON.FromJson(www.downloadHandler.text);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        //string[] _listTh = item.Sp

                        _theories.Add(item);
                        if (item.Number == indexCurrentTheory + 1)
                        {
                            _theoriesString.Add(item.Content);
                            countText = _theoriesString.Count;
                            NextType();

                        }
                        
                    }
                    Debug.Log(_theoriesString.Count);
                }

                InitTextTypeTest();
            }
        }
    }
}
