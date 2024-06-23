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
    private GameObject errorNullData; // Окно ошибки для пустых данных
    [SerializeField]
    private GameObject errorBadWords; // Окно ошибки для запрещенных слов
    [SerializeField]
    private GameObject errorEmail; // Окно ошибки для неверного формата электронной почты

    private HashSet<string> bannedWords;


    private void Start()
    {
        nameInputField.text = UserData.UserFullName;
        usernameInputField.text = UserData.UserName;
        emailInputField.text = UserData.UserEmail;
        passwordInputField.text = UserData.UserPassword;

        // Загрузим и разделим текстовые файлы
        TextAsset txtEnglishAsset = Resources.Load<TextAsset>("BadWords/en");
        TextAsset txtRussianAsset = Resources.Load<TextAsset>("BadWords/ru");
        TextAsset txtItalianAsset = Resources.Load<TextAsset>("BadWords/it");
        TextAsset txtGermanAsset = Resources.Load<TextAsset>("BadWords/ge");
        string txtEnglish = txtEnglishAsset.text;
        string txtRussian = txtRussianAsset.text;
        string txtItalian = txtItalianAsset.text;
        string txtGerman = txtGermanAsset.text;
        List<string> wordsEnglish = txtEnglish.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> wordsRussian = txtRussian.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> wordsItalian = txtItalian.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> wordsGerman = txtGerman.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        // Объединим списки слов и создадим HashSet для быстрой проверки
        bannedWords = new HashSet<string>(wordsEnglish.Concat(wordsRussian).Concat(wordsItalian).Concat(wordsGerman));
    }

    public void SaveData()
    {
        // Проверяем, что все поля заполнены
        if (string.IsNullOrEmpty(nameInputField.text) || string.IsNullOrEmpty(usernameInputField.text) || string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            errorNullData.SetActive(true);
            return;
        }

        // Проверяем ввод пользователя на наличие плохих слов
        string lowerCaseName = nameInputField.text.ToLower();
        string lowerCaseUsername = usernameInputField.text.ToLower();
        string lowerCaseEmail = emailInputField.text.ToLower();
        if (bannedWords.Any(word => lowerCaseName.Contains(word)) || bannedWords.Any(word => lowerCaseUsername.Contains(word)) || bannedWords.Any(word => lowerCaseEmail.Contains(word)))
        {
            errorBadWords.SetActive(true);
            return;
        }

        // Проверяем, что в поле электронной почты присутствует символ "@"
        if (!emailInputField.text.Contains("@"))
        {
            errorEmail.SetActive(true);
            return;
        }

        UserData.UserFullName = nameInputField.text;
        UserData.UserName = usernameInputField.text;
        UserData.UserEmail = emailInputField.text;
        UserData.UserPassword = Encrypt(passwordInputField.text);

        UserData.SaveUserRegData();

        StartCoroutine(WaitAndLoadScene(1, "DefaultPersonalPage"));
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

    IEnumerator WaitAndLoadScene(float waitTime, string sceneName)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("PersonalPageWithSettings");
    }
}
