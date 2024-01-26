using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class RegisterUserData : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private GameObject errorWindow;
    [SerializeField]
    private GameObject errorNullData; // Окно ошибки для пустых полей
    [SerializeField]
    private GameObject successRegistrationWindow; // Окно подтверждения регистрации
    [SerializeField]
    private Button okButton; // Кнопка "ОК" в окне подтверждения регистрации
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
        // Проверяем, что все поля заполнены
        if (string.IsNullOrEmpty(nameInputField.text) || string.IsNullOrEmpty(usernameInputField.text) || string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            ShowErrorNullData("Все поля должны быть заполнены");
            return;
        }

        System.Random rand = new System.Random();
        int randID = rand.Next(1, 999999);
        UserData.SetUserData(randID.ToString(), nameInputField.text, usernameInputField.text, emailInputField.text, Encrypt(passwordInputField.text), 1, 0, 0, 0);

        PlayerPrefs.Save();
        ShowSuccessRegistrationWindow();
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

    public void ShowErrorNullData(string message)
    {
        errorNullData.SetActive(true);
        errorNullData.transform.Find("Image/ErrorMessage").GetComponent<TMPro.TextMeshProUGUI>().text = message;
    }

    public void HideErrorWindow()
    {
        errorWindow.SetActive(false);
    }

    public void HideErrorNullData()
    {
        errorNullData.SetActive(false);
    }

    public void ShowSuccessRegistrationWindow()
    {
        successRegistrationWindow.SetActive(true);
    }

    public void OnOkButtonClicked()
    {
        StartCoroutine(WaitAndLoadScene(1, "DefaultPersonalPage"));
    }

    IEnumerator WaitAndLoadScene(float waitTime, string sceneName)
    {
        successRegistrationWindow.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("RegistrationScene");
    }
}
