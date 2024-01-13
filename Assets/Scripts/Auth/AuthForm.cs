using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AuthJson;
using AuthJsonNew;
using ResponseJson;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private TMP_InputField _emailField;
    [SerializeField] private TMP_InputField _passField;
    [SerializeField] private TMP_InputField _passRepeatField;
    [SerializeField] private bool _isLogForm;
    [SerializeField] private Image _imageAvatar;
    [SerializeField] private TMP_Text _textButtonEditSave;
    [SerializeField] private GameObject _buttonSelectAvatar;

    private void Start()
    {
        UserData.LoadUserData();
        if(_nameField)
            _nameField.text = UserData.UserName;
        _emailField.text = UserData.UserEmail;
        _passField.text = UserData.UserPassword;
        if(_imageAvatar)
            ComonFunctions.LoadAvatarFromResourceByID(_imageAvatar, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID);
        if(_textButtonEditSave)
            _textButtonEditSave.text = LangAsset.GetValueByKey("Edit");
        if(_buttonSelectAvatar)
            _buttonSelectAvatar.SetActive(false);
    }

    public void ClickOK()
    {
        if (_isLogForm)
            StartCoroutine(CheckDataForUser());
        else
        {
            CloseAuthScenes();
            SceneManager.LoadScene("LoginForm", LoadSceneMode.Additive);
        }
    }

    public void ClickCancel()
    {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    public void ClickRegistry()
    {
        if (_isLogForm)
            SceneManager.LoadScene("LoginRegForm", LoadSceneMode.Additive);
        else
            StartCoroutine(CheckDataForUser());
    }

    private IEnumerator CheckDataForUser()
    {
        string name = "";
        if (!_isLogForm)
        {
            name = _nameField.text;
            if (_nameField.text.Length < 1)
            {
                Debug.LogError("Input name is with errors");
                yield break;
            }
        }
        if (UserData.IsByVK == 0 && _emailField.text.Length < 7)
        {
            Debug.LogError("Input email is with errors");
            yield break;
        }
        if (UserData.IsByVK == 0 && _passField.text.Length < 7)
        {
            Debug.LogError("Input pass is with errors");
            yield break;
        }
        if (!_isLogForm && _passRepeatField != null && _passField.text != _passRepeatField.text)
        {
            Debug.LogError("Pass 1 and pass 2 are not equal");
            yield break;
        }


        string email = _emailField.text;
        string password = _passField.text;
        if (_isLogForm)
            StartCoroutine(LoginUser(email, password));
        else
        {
            if (UserData.UserID == "")
                StartCoroutine(CreateNewUser(name, email, password, 0, 0, 0, UserData.Score));
            else
                StartCoroutine(ComonFunctions.Instance.UpdateUser(UserData.UserID, name, email, password, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID, UserData.Score));
        }
        yield break;
    }

    private void SignInUser(string email, string password)
    {
        //AuthInit.GetInstance().SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        //{
        //    if (task.IsCanceled)
        //    {
        //        Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
        //        return;
        //    }
        //    if (task.IsFaulted)
        //    {
        //        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
        //        return;
        //    }

        //    Firebase.Auth.AuthResult result = task.Result;
        //    Debug.LogFormat("User signed in successfully: {0} ({1})",
        //        result.User.DisplayName, result.User.UserId);
        //});
    }

    private IEnumerator CreateNewUser(string name, string email, string password, int avatarID, int isByVK, int VKID, int score)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("accept", "application/ld+json");
        headers.Add("Content-Type", "application/ld+json");

        //WWWForm form = new WWWForm();
        
        //form.AddField("login", name);
        //form.AddField("plainPassword", email);
        //form.headers.Add("accept", "application/ld+json");
        //form.headers.Add("Content-Type", "application/ld+json");


        //form.AddField("password", password);
        //form.AddField("avatarID", avatarID);
        //form.AddField("isByVK", isByVK);
        //form.AddField("VKID", VKID);
        //form.AddField("score", score);
        string strPass = "";
        AuthPost auth = new AuthPost();
        auth.login = email;
        auth.plainPassword = password;
        strPass = JsonUtility.ToJson(auth);

        var body = Encoding.UTF8.GetBytes(strPass);

        UnityWebRequest www = UnityWebRequest.Put("https://teach-app.pyatnitsev.ru/api/users/create", body);
        
        www.SetRequestHeader("Accept", "application/ld+json");
        www.SetRequestHeader("Content-Type", "application/ld+json");
        www.method = "POST";

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            ResponseAuthNew nativeResponse = ResponseAuthNew.FromJson(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);
            if (nativeResponse != null)
            {
                //if (int.TryParse(nativeResponse.UserID, out int resultID))
                //{

                UserData.UserID = nativeResponse.UserID;
                //Debug.Log(UserData.UserID);
                UserData.SetUserData(UserData.UserID, name, name, email, password, avatarID, isByVK, VKID, score);
                CloseAuthScenes();
                SceneManager.LoadScene("UserForm", LoadSceneMode.Additive);
                //}
            }

        }
    }

    private IEnumerator CreateNewUserOld(string name, string email, string password, int avatarID, int isByVK, int VKID, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("fullName", name);
        form.AddField("userEmail", email);
        form.AddField("password", password);
        form.AddField("avatarID", avatarID);
        form.AddField("isByVK", isByVK);
        form.AddField("VKID", VKID);
        form.AddField("score", score);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/insert_user.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            ResponseCode nativeResponse = ResponseCode.FromJson(www.downloadHandler.text);
            if (nativeResponse.ResponseCodeValue == 1)
            {
                if (int.TryParse(nativeResponse.ResponseData, out int resultID))
                {
                    UserData.UserID = resultID.ToString(); ;
                    UserData.SetUserData(UserData.UserID, name, name, email, password, avatarID, isByVK, VKID, score);
                    CloseAuthScenes();
                    SceneManager.LoadScene("UserForm", LoadSceneMode.Additive);
                }
            }

        }
    }

    //private IEnumerator UpdateUser(int userID, string name, string email, string password, int avatarID, int isByVK, int VKID, int score)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("id", userID);
    //    form.AddField("fullName", name);
    //    form.AddField("userEmail", email);
    //    form.AddField("password", password);
    //    form.AddField("avatarID", avatarID);
    //    form.AddField("isByVK", isByVK);
    //    form.AddField("VKID", VKID);
    //    form.AddField("score", score);

    //    UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/update_user.php", form);
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Debug.Log("Form upload complete!");
    //        Debug.Log(www.downloadHandler.text);
    //        UserData.SetUserData(userID, name, email, password, avatarID, isByVK, VKID, score);
    //    }
    //}

    private IEnumerator LoginUser(string email, string password)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("accept", "application/ld+json");
        headers.Add("Content-Type", "application/ld+json");

        //WWWForm form = new WWWForm();
        //form.AddField("userEmail", email);
        //form.AddField("password", password);

        //UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/login_user.php", form);

        string strPass = "";
        LoginPost login = new LoginPost();
        login.login = email;
        login.password = password;
        strPass = JsonUtility.ToJson(login);

        var body = Encoding.UTF8.GetBytes(strPass);

        UnityWebRequest www = UnityWebRequest.Put("https://teach-app.pyatnitsev.ru/api/login", body);

        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Content-Type", "application/json");
        www.method = "POST";

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            ResponseAuthNew nativeResponse = ResponseAuthNew.FromJson(www.downloadHandler.text);
            if (nativeResponse != null)
            {
                UserData.SetToken(nativeResponse.Token);
                StartCoroutine(ComonFunctions.Instance.GetUserTeamID(UserData.UserID.ToString()));
                SceneManager.LoadScene("UserForm", LoadSceneMode.Additive);
            }


        }
    }

    public void ClickChangeUserData()
    {
        if (_textButtonEditSave.text == LangAsset.GetValueByKey("Edit"))
        {
            _textButtonEditSave.text = LangAsset.GetValueByKey("Save");
            _buttonSelectAvatar.SetActive(true);
        }
        else
        {
            _isLogForm = false;
            _textButtonEditSave.text = LangAsset.GetValueByKey("Edit");
            StartCoroutine(CheckDataForUser());
            _buttonSelectAvatar.SetActive(false);
        }
    }

    public void SetAvatarImageByID(int avatarID)
    {
        UserData.UserAvatarID = avatarID;
        if(avatarID == 0 && UserData.VKID > 0)
            ComonFunctions.LoadAvatarFromResourceByID(_imageAvatar, UserData.UserAvatarID, UserData.VKID, UserData.VKID);
        else
            ComonFunctions.LoadAvatarFromResourceByID(_imageAvatar, UserData.UserAvatarID);
    }

    public void ClickLogOut()
    {
        UserData.SetUserData("",
                   "",
                   "",
                   "",
                   "",
                   0,
                   0,
                   0,
                   0);
        ComonFunctions.LoadAvatarFromResourceByID(_imageAvatar, UserData.UserAvatarID);
        CloseAuthScenes();
        SceneManager.LoadScene("LoginForm", LoadSceneMode.Additive);
    }

    private void CloseAuthScenes()
    {
        foreach (var item in SceneManager.GetAllScenes())
        {
            if (item.name != "MapScene")
                SceneManager.UnloadSceneAsync(item);
        }
    }

}
