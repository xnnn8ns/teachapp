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


    private void Start()
    {
        nameInputField.text = UserData.UserFullName;
        usernameInputField.text = UserData.UserName;
        emailInputField.text = UserData.UserEmail;
        passwordInputField.text = UserData.UserPassword;
    }

    public void SaveData()
    {
        UserData.UserFullName = nameInputField.text;
        UserData.UserName = usernameInputField.text;
        UserData.UserEmail = emailInputField.text;
        UserData.UserPassword = Encrypt(passwordInputField.text);

        UserData.SaveUserRegData();
    }

    private string Encrypt(string str)
    {
        char[] charArray = str.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            charArray[i] = (char)(charArray[i] ^ 129);
        }
        return new string(charArray);
    }

    public void DeleteAccount()
    {
        UserData.DeleteAllData();
    }

}
