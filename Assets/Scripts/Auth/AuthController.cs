using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VKJson;

public class AuthController : MonoBehaviour
{
    public UniWebView webView;
    private static GameObject webViewGameObject;
    [SerializeField]
    private Image imagePerson;

    void Start()
    {
        //ExecuteAuth();
    }

    public void ExecuteAuth()
    {
        // Debug.LogError("Cube-Log ExecuteAuth");

        webViewGameObject = GetWebViewGameObject();

        webViewGameObject.SetActive(true);
        gameObject.SetActive(true);

        webView = CreateUniWebView();

        webView.CleanCache();

        webView.Load("http://sg12ngec.beget.tech/auth/auth.html", true);

            webView.SetShowToolbar(
                true,  // Show or hide?         true  = show
                false, // With animation?       false = no animation
                false,  // Is it on top?         true  = top
                false  // Should adjust insets? true  = avoid overlapping to web view
            );
            webView.Alpha = 1.0f;


        webView.Show(true);

        webView.OnMessageReceived += (view, message) => RecieveMessage(view, message);

        webView.OnPageFinished += (view, statusCode, url) =>
        {
            //Debug.Log(url);
            OnPageLoad(url,statusCode);
        };
    }

    public static GameObject GetWebViewGameObject()
    {
        if (webViewGameObject == null)
            webViewGameObject = new GameObject("UniWebView");
        return webViewGameObject;
    }

    private UniWebView CreateUniWebView()
    {
        if (webView == null)
        {
            webView = webViewGameObject.AddComponent<UniWebView>();
            webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
            webView.SetSupportMultipleWindows(true);
            webView.SetAllowBackForwardNavigationGestures(true);
            webView.SetBackButtonEnabled(true);
            webView.SetZoomEnabled(true);
            webView.SetToolbarDoneButtonText(LangAsset.GetValueByKey("Close"));
            webView.SetToolbarGoBackButtonText(LangAsset.GetValueByKey("Back"));
            webView.SetToolbarGoForwardButtonText(LangAsset.GetValueByKey("Forward"));
            webView.OnMessageReceived += (view, message) => RecieveMessage(view, message);
        }

        return webView;
    }

    private void RecieveMessage(UniWebView webView, UniWebViewMessage message)
    {
        try
        {
            if (message.Path.Equals("test"))
            {
                
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error: " + ex.Message);
        }
    }

    private void OnPageLoad(string url, int statusCode = 200)
    {
        Debug.Log("OnPageFinished: " + statusCode);
        if (statusCode == 200)
            ParseParams(url);
        else
            CloseWebForm();
    }

    private void ParseParams(string value)
    {
        value = value.Replace('#', '?');
        Uri myUri = new Uri(value);
        Debug.Log(value);
        string user_id = HttpUtility.ParseQueryString(myUri.Query).Get("user_id");
        if (user_id != null && user_id != "null")
        {
            string access_token = HttpUtility.ParseQueryString(myUri.Query).Get("access_token");
            Debug.Log(access_token);
            Debug.Log(user_id);
            StartCoroutine(GetVKUserID(user_id, access_token));
            //CloseWebForm();
        }
        
    }

    private void CloseWebForm()
    {
        if (webViewGameObject)
        {
            webView.SetShowToolbar(
                false,  // Show or hide?         true  = show
                false, // With animation?       false = no animation
                true,  // Is it on top?         true  = top
                false  // Should adjust insets? true  = avoid overlapping to web view
            );
            webViewGameObject?.SetActive(false);
            //gameObject?.SetActive(false);
            webView.gameObject.SetActive(false);
        }
    }

    public IEnumerator GetVKUserID(string userID, string accessToken)
    {
        string url = "";
        if (userID.Length > 0)
            url = string.Format("https://api.vk.com/method/users.get?user_ids={0}&fields=photo_50&access_token={1}&v=5.101", userID, accessToken);
        else
            yield break;

        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(www.url);
        www.timeout = 5;
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.text);
            VKResponse vkResponse = VKResponse.FromJson(www.downloadHandler.text);
            if(vkResponse.ResponseList != null && vkResponse.ResponseList.Count > 0)
                StartCoroutine(ComonFunctions.Instance.GetIconFromURLByUserID(vkResponse.ResponseList[0].UserID, vkResponse.ResponseList[0].UserPhoto, imagePerson));
        }
        else
        {
            Debug.Log(www.error);
        }
        CloseWebForm();
    }

    public IEnumerator GetVKLogout(string userID, string accessToken)
    {
        string url = "";
        if (userID.Length > 0)
            url = string.Format("https://api.vk.com/method/users.get?user_ids={0}&fields=photo_50&access_token={1}&v=5.101", userID, accessToken);
        else
            yield break;

        UnityWebRequest www = UnityWebRequest.Get(url);
        Debug.Log(www.url);
        www.timeout = 5;
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.text);
            VKResponse vkResponse = VKResponse.FromJson(www.downloadHandler.text);
            if (vkResponse.ResponseList != null && vkResponse.ResponseList.Count > 0)
                StartCoroutine(ComonFunctions.Instance.GetIconFromURLByUserID(vkResponse.ResponseList[0].UserID, vkResponse.ResponseList[0].UserPhoto, imagePerson));
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    public void ClickAuthByVK()
    {
        ExecuteAuth();
    }
}
