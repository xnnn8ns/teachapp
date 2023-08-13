using System.Collections;
using System.Collections.Generic;
using AuthJson;
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
        _nameField.text = UserData.UserName;
        _emailField.text = UserData.UserEmail;
        _passField.text = UserData.UserPassword;
        LoadAvatarFromResourceByID(UserData.UserAvatarID);
        _textButtonEditSave.text = LangAsset.GetValueByKey("Edit");
        _buttonSelectAvatar.SetActive(false);
    }

    private void LoadAvatarFromResourceByID(int avatarID)
    {
        _imageAvatar.sprite = Resources.Load<Sprite>("Avatars/" + avatarID.ToString());
    }

    public void ClickOK()
    {
        if(_isLogForm)
            StartCoroutine(CheckDataForUser());
        else
            SceneManager.LoadScene("LoginForm", LoadSceneMode.Single);
    }

    public void ClickCancel()
    {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    public void ClickRegistry()
    {
        if(_isLogForm)
            SceneManager.LoadScene("LoginRegForm", LoadSceneMode.Single);
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
        if (_emailField.text.Length < 7)
        {
            Debug.LogError("Input email is with errors");
            yield break;
        }
        if (_passField.text.Length < 7)
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
            if(UserData.UserID == 0)
                StartCoroutine(CreateNewUser(name, email, password, 0));
            else
                StartCoroutine(UpdateUser(UserData.UserID, name, email, password, UserData.UserAvatarID));
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

    private IEnumerator CreateNewUser(string name, string email, string password, int avatarID)
    {
        WWWForm form = new WWWForm();
        form.AddField("fullName", name);
        form.AddField("userEmail", email);
        form.AddField("password", password);
        form.AddField("avatarID", avatarID);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/insert_user.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
        }
    }

    private IEnumerator UpdateUser(int userID, string name, string email, string password, int avatarID)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", userID);
        form.AddField("fullName", name);
        form.AddField("userEmail", email);
        form.AddField("password", password);
        form.AddField("avatarID", avatarID);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/update_user.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
            UserData.SetUserData(userID, name, email, password, avatarID);
        }
    }

    private IEnumerator LoginUser(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("userEmail", email);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/login_user.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            NativeResponseAuth nativeResponse = NativeResponseAuth.FromJson(www.downloadHandler.text);
            if(nativeResponse != null && nativeResponse.ResponseCode == 1)
            {
                UserData.SetUserData(nativeResponse.ResponseAuth[0].UserID,
                    nativeResponse.ResponseAuth[0].UserFullName,
                    nativeResponse.ResponseAuth[0].UserEmail,
                    nativeResponse.ResponseAuth[0].UserPassword,
                    nativeResponse.ResponseAuth[0].UserAvatarID);
                SceneManager.LoadScene("UserForm", LoadSceneMode.Single);
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
        LoadAvatarFromResourceByID(UserData.UserAvatarID);
    }
}
