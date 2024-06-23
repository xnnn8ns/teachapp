using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using TMPro;
using System;
using System.Globalization;

public class GetCertificateFromServer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameInputField;

    public Action _action;
    //void Start()
    //{
    //    //GetWriteNewCertificate();
    //    GetHtmlFile("pdfpage.html");

    //}

    public void GetHtmlFile(Action action, string filePath = "pdfpage.html")
    {
        //**************************************** Comment for a time
        _action = action;
        StartCoroutine(PostHtmlFile(filePath));
    }

    private string GetWriteNewCertificate()
    {
        //string filePath = Application.dataPath + "/Resources/certificate/pdfpage.html";
        TextAsset asset;
        if (LangAsset.CurrentLangLocation != LangLocation.Ru)
            asset = (TextAsset)Resources.Load<TextAsset>("certificate/pdfpage");
        else
            asset = (TextAsset)Resources.Load<TextAsset>("certificate/pdfpage-ru");
        List<string> lines = new List<string>(asset.text.Split('\n'));
        //List<string> linesUpdated = new List<string>();
        Debug.Log(lines.Count);
        string linesUpdated = "";
        foreach (var item in lines)
        {
            string updatedStr = item;
            if (item.Contains(">Title<"))
            {
                string[] strBlocks = item.Split(">");
                string newStr = strBlocks[0] + ">";
                string[] strBlocksLast = strBlocks[1].Split("<");
                newStr += nameInputField.text;
                newStr += "<" + strBlocksLast[1] + ">";
                updatedStr = newStr;
                Debug.Log(updatedStr);
            }
            if (item.Contains("DateCertificate"))
            {
                string[] strBlocks = item.Split("<br>");
                string newStr = strBlocks[0] + "<br>";
                string[] strBlocksLast = strBlocks[1].Split("<");

                DateTime now = DateTime.Now;
                if (LangAsset.CurrentLangLocation != LangLocation.Ru)
                    newStr += now.ToString("D", CultureInfo.CreateSpecificCulture("EN-us"));
                else
                    newStr += now.ToString("D", CultureInfo.CreateSpecificCulture("RU-ru"));

                //newStr += DateTime.Today.ToLongDateString().Replace(DateTime.Now.DayOfWeek.ToString() + ", ", "");
                newStr += "<" + strBlocksLast[1];// + ">";
                updatedStr = newStr;
                Debug.Log(updatedStr);
            }
            //linesUpdated.Add(updatedStr);
            linesUpdated += updatedStr;
        }
        return linesUpdated;
    }

    IEnumerator PostHtmlFile(string filePath)
    {
        string url = "https://d5dlcdt4eicv4aq51qqd.apigw.yandexcloud.net/";
        //string fullPath = Path.Combine(Application.persistentDataPath, "Resources/certificate", filePath);
        Debug.Log(filePath);

        // string htmlContent = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(fullPath)).text;
        //string htmlContent = Resources.Load<TextAsset>("certificate/pdfpage").text;
        string htmlContent = GetWriteNewCertificate();
        Debug.Log(htmlContent);
        WWWForm form = new WWWForm();
        form.AddField("html", htmlContent);
        Debug.Log("html"); 
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("HTML file successfully sent to server: " + webRequest.downloadHandler.text);
                string urlPDF = GetURLFromJSONResponse(webRequest.downloadHandler.text);
                Debug.Log(urlPDF);
                PlayerPrefs.SetString("Certificate1", urlPDF);
                _action.Invoke();
                //var file = webRequest.downloadHandler.data;
                //FileStream nFile = new FileStream(Application.persistentDataPath + "/certificate-python.pdf", FileMode.Create);
                //nFile.Write(file, 0, file.Length);
                //nFile.Close();
            }
        }
        Debug.Log("end"); 
    }

    public static string GetURLFromJSONResponse(string json)
    {
        //string str = json.Split(":")[1];
        string str = json.Substring(9, json.Length - 11);
        str = str.Replace(@"\/", "/");
        return str;
    }
}
