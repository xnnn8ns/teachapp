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
    [Serializable]
    public class userData
    {
        public int id;
        public string name;
        public string username;
        public string email;
        public string password;
        public int levelsCompleted;
        public int score;
        public int rank;
        public string startTime;
        public int initialLevelsCompleted;
        public int initialScore;
        public string lastUpdateDate;
        public string elapsedTime;
    }

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
    [SerializeField]
    private Button registerButton;

    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.dataPath, "Resources", "userData.json");
        registerButton.onClick.AddListener(RegisterData);
    }

    public void RegisterData()
    {
        List<userData> users = new List<userData>();

        if (File.Exists(filePath))
        {
            string existingData = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(existingData) && existingData.StartsWith("[") && existingData.EndsWith("]"))
            {
                users = JsonConvert.DeserializeObject<List<userData>>(existingData);
            }
        }

        if (users.Any(user => user.username == usernameInputField.text || user.email == emailInputField.text))
        {
            ShowErrorWindow("Пользователь с таким именем пользователя или электронной почтой уже существует");
            return;
        }

        int newId = users.Count > 0 ? users.Max(user => user.id) + 1 : 1;
        userData newUser = new userData
        {
            id = newId,
            name = nameInputField.text,
            username = usernameInputField.text,
            email = emailInputField.text,
            password = Encrypt(passwordInputField.text),
            levelsCompleted = 0,
            score = 0,
            rank = 0,
            startTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString(),
            initialLevelsCompleted = 0,
            initialScore = 0,
            lastUpdateDate = DateTime.Now.ToString("yyyy-MM-dd"),
            elapsedTime = "0"
        };

        users.Add(newUser);
        PlayerPrefs.SetInt("id", newUser.id);

        string userId = PlayerPrefs.GetInt("id").ToString();

        PlayerPrefs.Save();

        string updatedData = JsonConvert.SerializeObject(users);

        File.WriteAllText(filePath, updatedData);
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
