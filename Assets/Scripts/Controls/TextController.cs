using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ResponseTheoryJson;
using ResponseTheoryListJSON;
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
    private TMP_Text theoryTitle1Text;
    [SerializeField]
    private TMP_Text theoryTitle2Text;
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
        theoryTitle1Text.text = LangAsset.GetValueByKey("TheorySection");
        theoryTitleText.text = LangAsset.GetValueByKey("Section") + " " + (Settings.Current_Topic).ToString();
        //theoryTitle2Text.text = LangAsset.GetValueByKey("Section") + " " + (Settings.Current_Topic).ToString();
        string json = File.ReadAllText(Application.persistentDataPath + Settings.jsonTheoryFilePath);
        TheoryListJSON _theoryListJSON = JsonConvert.DeserializeObject<TheoryListJSON>(json);
        foreach (var item in _theoryListJSON.theoryList)
        {
            if (item.ID == Settings.Current_Topic)
            {
                theoryTitle2Text.text = item.Description;
                break;
            }
        }
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
        string lang = "en";
        if(LangAsset.CurrentLangLocation == LangLocation.En)
            lang = "en";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ru)
            lang = "ru";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ge)
            lang = "ge";

        //string strType = Resources.Load<TextAsset>("HTML_Theory/" + lang + "/th_" + (Settings.Current_Topic).ToString()).text;
        string strType = Theory.TheoryList[Settings.Current_Topic - 1].Description;
        _textCurrentForType.ShowFullText(strType, NextType);
    }

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

    private bool OnShouldCloseWebView(UniWebView webView)
    {
        ClickReturnFromTheory();
        return true;
    }

    public void ClickReturnFromTheory()
    {
        //_clickAudio?.Play();
        Vibration.VibratePop();
        //SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "WebWidget")
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
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
