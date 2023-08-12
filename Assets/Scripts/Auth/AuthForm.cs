using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private TMP_InputField _emailField;
    [SerializeField] private TMP_InputField _passField;
    [SerializeField] private TMP_InputField _passRepeatField;
    [SerializeField] private bool _isLogForm;


    public void ClickOK()
    {
        if(_isLogForm)
            StartCoroutine(SignInUser());
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
            StartCoroutine(SignInUser());
    }

    private IEnumerator SignInUser()
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
        if (!_isLogForm && _passField.text != _passRepeatField.text)
        {
            Debug.LogError("Pass 1 and pass 2 are not equal");
            yield break;
        }

         
        string email = _emailField.text;
        string password = _passField.text;
        if (_isLogForm)
            StartCoroutine(LoginUser(email, password));
        else
            StartCoroutine(CreateNewUser(name, email, password));
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

    private IEnumerator CreateNewUser(string name, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("fullName", name);
        form.AddField("userEmail", email);
        form.AddField("password", password);

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
        }
    }
}
