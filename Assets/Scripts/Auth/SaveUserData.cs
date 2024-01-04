using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;

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

    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.dataPath, "Resources/userData.json");

        nameInputField.text = PlayerPrefs.GetString("name", "");
        usernameInputField.text = PlayerPrefs.GetString("username", "");
        emailInputField.text = PlayerPrefs.GetString("email", "");
        passwordInputField.text = PlayerPrefs.GetString("password", "");
        savebutton.onClick.AddListener(SaveData);
    }

    public void SaveData()
    {
        List<userData> users = new List<userData>();

        if (File.Exists(filePath))
        {
            string existingData = File.ReadAllText(filePath);
            users = JsonConvert.DeserializeObject<List<userData>>(existingData);
        }

        userData existingUser = users.Find(user => user.username == usernameInputField.text);

        if (existingUser != null)
        {
            // Обновляем данные существующего пользователя
            existingUser.name = nameInputField.text;
            existingUser.email = emailInputField.text;
            existingUser.password = Encrypt(passwordInputField.text);
            PlayerPrefs.SetInt("id", existingUser.id);
        }
        else
        {
            // Добавляем нового пользователя
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
                rank = 0
            };

            users.Add(newUser);
            PlayerPrefs.SetInt("id", newUser.id);
        }
        Debug.Log($"Установка id в PlayerPrefs: {PlayerPrefs.GetInt("id")}");
        PlayerPrefs.Save();
        Debug.Log($"Текущий id в PlayerPrefs: {PlayerPrefs.GetInt("id")}");

        string updatedData = JsonConvert.SerializeObject(users);

        File.WriteAllText(filePath, updatedData);

        PlayerPrefs.SetString("name", nameInputField.text);
        PlayerPrefs.SetString("username", usernameInputField.text);
        PlayerPrefs.SetString("email", emailInputField.text);
        PlayerPrefs.SetString("password", passwordInputField.text);
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
}

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
}