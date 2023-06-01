using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthForm : MonoBehaviour
{
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
        string passwordRepeat = _passRepeatField.text;
        if (_isLogForm)
            SignInUser(email, password);
        else
            CreateNewUser(email, password);
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

    private void CreateNewUser(string email, string password)
    {
        //AuthInit.GetInstance().CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        //{
        //    if (task.IsCanceled)
        //    {
        //        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
        //        return;
        //    }
        //    if (task.IsFaulted)
        //    {
        //        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
        //        return;
        //    }

        //    // Firebase user has been created.
        //    Firebase.Auth.AuthResult result = task.Result;
        //    Debug.LogFormat("Firebase user created successfully: {0} ({1})",
        //        result.User.DisplayName, result.User.UserId);
        //});
    }

}
