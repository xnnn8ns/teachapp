using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

[System.Serializable]
public class User
{
    public string name;
    public int score;
}

[System.Serializable]
public class UserList
{
    public User[] users;
}

public class JsonLoad : MonoBehaviour
{
    public TMP_Text[] nameUsers;
    public TMP_Text[] scoreUsers;
    void Start()
    {
        LoadUserDataFromJson();
    }
    void LoadUserDataFromJson()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("user");
        if (textAsset == null)
        {
            Debug.LogError("Failed to load text asset 'user'");
            return;
        }

        string json = textAsset.text;
        UserList userList = JsonUtility.FromJson<UserList>(json);
        if (userList == null || userList.users == null)
        {
            Debug.LogError("Failed to parse JSON into UserList");
            return;
        }

        for (int i = 0; i < nameUsers.Length && i < scoreUsers.Length; i++)
        {
            if (i < userList.users.Length)
            {
                TMP_Text nameText = nameUsers[i];
                TMP_Text scoreText = scoreUsers[i];

                if (nameText == null)
                {
                    Debug.LogError("NameUser at index " + i + " is null");
                    continue;
                }

                if (scoreText == null)
                {
                    Debug.LogError("ScoreUser at index " + i + " is null");
                    continue;
                }

                nameText.text = userList.users[i].name;
                scoreText.text = userList.users[i].score.ToString();
            }
        }
    }
}