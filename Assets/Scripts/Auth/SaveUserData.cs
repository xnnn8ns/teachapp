using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class SaveUserData : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private TMP_InputField usernameInputField;
    [SerializeField]
    private TMP_InputField emailInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
    [SerializeField]
    private Button savebutton;


    private void Start()
    {
        nameInputField.text = UserData.UserFullName;
        usernameInputField.text = UserData.UserName;
        emailInputField.text = UserData.UserEmail;
        passwordInputField.text = UserData.UserPassword;
        savebutton.onClick.AddListener(SaveData);
    }

    public void SaveData()
    {
        UserData.UserFullName = nameInputField.text;
        UserData.UserName = usernameInputField.text;
        UserData.UserEmail = emailInputField.text;
        UserData.UserPassword = passwordInputField.text;

        UserData.SaveUserRegData();
    }

}
