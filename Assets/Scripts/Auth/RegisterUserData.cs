using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class RegisterUserData : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private GameObject errorWindow;
    [SerializeField]
    private TMP_InputField usernameInputField;
    [SerializeField]
    private TMP_InputField emailInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;

    private void Start()
    {
    }

    public void RegisterData()
    {
        System.Random rand = new System.Random();
        int randID = rand.Next(1, 999999);
        UserData.SetUserData(randID.ToString(), nameInputField.text, usernameInputField.text, emailInputField.text, Encrypt(passwordInputField.text), 1, 0, 0, 0);

        PlayerPrefs.Save();
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

    public void ShowErrorWindow(string message)
    {
        errorWindow.SetActive(true);
        errorWindow.transform.Find("Image/ErrorMessage").GetComponent<TMPro.TextMeshProUGUI>().text = message;
    }

    public void HideErrorWindow()
    {
        errorWindow.SetActive(false);
    }

}
