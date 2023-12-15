using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

public class JsonLoadUser : MonoBehaviour
{
    public TMP_Text[] nameUsers;
    public TMP_Text[] scoreUsers;

    private const string UserFileName = "userList";

    void Start()
    {
        LoadUserDataFromJson();
    }

    void LoadUserDataFromJson()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(UserFileName);
        if (textAsset == null)
        {
            Debug.LogError($"Failed to load text asset '{UserFileName}'");
            return;
        }

        UserList userList = JsonUtility.FromJson<UserList>(textAsset.text);
        if (userList == null || userList.users == null)
        {
            Debug.LogError("Failed to parse JSON into UserList");
            return;
        }

        UpdateUserTexts(userList);
    }

    void UpdateUserTexts(UserList userList)
    {
        int userCount = userList.users.Length;
        for (int i = 0; i < userCount; i++)
        {
            if (nameUsers[i] == null || scoreUsers[i] == null)
            {
                Debug.LogError($"Text component at index {i} is null");
                continue;
            }

            nameUsers[i].text = userList.users[i].name;
            scoreUsers[i].text = userList.users[i].score.ToString();
        }
    }
}