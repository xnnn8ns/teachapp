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
    private GameObject errorWindow; // не используется (сообщение о том, что пользователь с таким именем уже существует)
    [SerializeField]
    private GameObject errorNullData; // Окно ошибки для пустых полей
    [SerializeField]
    private GameObject errorBadWords; // Окно ошибки для плохих слов
    [SerializeField]
    private GameObject errorEmail; // Окно ошибки для неверного формата электронной почты
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
    private HashSet<string> bannedWords;

    private void Start()
    {
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

    public void RegisterData()
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

        System.Random rand = new System.Random();
        int randID = rand.Next(1, 999999);
        
        // в SetUserData сначала сохраняет ник, затем полное имя; Изменил порядок переменных
        UserData.SetUserData(randID.ToString(), usernameInputField.text, nameInputField.text, emailInputField.text, Encrypt(passwordInputField.text), 1, 0, 0, 0);

        PlayerPrefs.Save();
        successRegistrationWindow.SetActive(true);
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
