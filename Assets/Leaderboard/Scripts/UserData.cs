using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class UserDataJson
{
    public int id;
    public string username;
    public string password;
    public int levelsCompleted;
    public int score;
    public int rank;
}

public class UserData : MonoBehaviour
{
    string jsonFilePath = "/Resources/userData.json";
    public TMP_InputField inputFieldUsername;
    public TMP_InputField inputFieldPassword;

    private UserDataJson UserDataJson = new UserDataJson();


    public void RegisterUserSaveData()
    {
        UserDataJson.username = inputFieldUsername.text;
        UserDataJson.password = inputFieldPassword.text;
        UserDataJson.id = 1;
        UserDataJson.levelsCompleted = 0;
        UserDataJson.score = 0;
        UserDataJson.rank = 0;

        string jsonData = JsonConvert.SerializeObject(UserDataJson, Formatting.Indented);
        File.WriteAllText(Application.dataPath + jsonFilePath, jsonData);

        Debug.Log("Данные сохранены в файл userData.json");
    }


    private void UpdateUI()
    {
        Debug.Log( "Имя пользователя: " + UserDataJson.username);
        Debug.Log("Пройденные уровни: " + UserDataJson.levelsCompleted);
        Debug.Log("Очки: " + UserDataJson.score);
        Debug.Log("Ранг: " + UserDataJson.rank);
    }
}
