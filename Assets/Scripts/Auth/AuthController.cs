using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using AuthJson;
using ResponseJson;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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

        //webView = CreateUniWebView();

        //webView.CleanCache();

        webView.Load("http://sg12ngec.beget.tech/auth/auth.html", true);

            webView.SetShowToolbar(
                false,  // Show or hide?         true  = show
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
        //if (webView == null)
        //{
            //webView = webViewGameObject.AddComponent<UniWebView>();
            //webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
            //webView.SetSupportMultipleWindows(true);
            //webView.SetAllowBackForwardNavigationGestures(true);
            //webView.SetBackButtonEnabled(true);
            //webView.SetZoomEnabled(true);
            //webView.SetToolbarDoneButtonText(LangAsset.GetValueByKey("Close"));
            //webView.SetToolbarGoBackButtonText(LangAsset.GetValueByKey("Back"));
            //webView.SetToolbarGoForwardButtonText(LangAsset.GetValueByKey("Forward"));
            //webView.OnMessageReceived += (view, message) => RecieveMessage(view, message);
        //}

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
            //webView.SetShowToolbar(
            //    false,  // Show or hide?         true  = show
            //    false, // With animation?       false = no animation
            //    true,  // Is it on top?         true  = top
            //    false  // Should adjust insets? true  = avoid overlapping to web view
            //);
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
        //Debug.Log(www.url);
        www.timeout = 5;
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(www.downloadHandler.text);
            VKResponse vkResponse = VKResponse.FromJson(www.downloadHandler.text);
            if (vkResponse.ResponseList != null && vkResponse.ResponseList.Count > 0)
            {
                string fullName = vkResponse.ResponseList[0].UserFirstName;
                if (vkResponse.ResponseList[0].UserLastName.Length > 0)
                {
                    if (fullName.Length > 0)
                        fullName += " ";
                    fullName += vkResponse.ResponseList[0].UserLastName;
                }
                UserData.SetUserData("", fullName, fullName, "", "", 0, 1, int.Parse(vkResponse.ResponseList[0].UserID), UserData.Score);
                StartCoroutine(LogByVK(fullName, "", "", 0, UserData.IsByVK, UserData.VKID, UserData.Score));
                StartCoroutine(ComonFunctions.Instance.GetIconFromURLByUserID(vkResponse.ResponseList[0].UserID, vkResponse.ResponseList[0].UserPhoto, imagePerson));
            }
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

    private IEnumerator LogByVK(string name, string email, string password, int avatarID, int isByVK, int VKID, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("fullName", name);
        form.AddField("userEmail", email);
        form.AddField("password", password);
        form.AddField("avatarID", avatarID);
        form.AddField("isByVK", isByVK);
        form.AddField("VKID", VKID);
        form.AddField("score", score);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/login_by_vk.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("LogByVK complete!");
            ResponseCode response = ResponseCode.FromJson(www.downloadHandler.text);

            if (response != null && response.ResponseCodeValue == 1)
            {
                if (int.TryParse(response.ResponseData, out int resultID))
                {
                    UserData.UserID = resultID.ToString();
                    UserData.SetUserData(UserData.UserID, name, name, email, password, avatarID, isByVK, VKID, score);
                    StartCoroutine(ComonFunctions.Instance.GetUserTeamID(UserData.UserID));

                    SceneManager.LoadScene("UserForm", LoadSceneMode.Additive);
                }
            }else if (response == null || response.ResponseCodeValue == 2)
            {
                NativeResponseAuth nativeResponse = NativeResponseAuth.FromJson(www.downloadHandler.text);
                if (nativeResponse != null)
                {
                    UserData.SetUserData(nativeResponse.ResponseAuth[0].UserID.ToString(),
                        nativeResponse.ResponseAuth[0].UserFullName,
                        nativeResponse.ResponseAuth[0].UserFullName,
                        nativeResponse.ResponseAuth[0].UserEmail,
                        nativeResponse.ResponseAuth[0].UserPassword,
                        nativeResponse.ResponseAuth[0].UserAvatarID,
                        nativeResponse.ResponseAuth[0].IsByVK,
                        nativeResponse.ResponseAuth[0].VKID,
                        nativeResponse.ResponseAuth[0].Score);
                    StartCoroutine(ComonFunctions.Instance.GetUserTeamID(UserData.UserID));
                    SceneManager.LoadScene("UserForm", LoadSceneMode.Additive);
                }
            }

        }
    }
}
