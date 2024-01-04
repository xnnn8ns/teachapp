using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class NameDataLoad : MonoBehaviour
{
    [System.Serializable]
    public class userData
    {
        public int id;
        public string username;
        public string name;
    }
    [SerializeField]
    private TMP_Text usernameText;
    [SerializeField]
    private TMP_Text nameText;

    private void Start()
    {
        LoadUserData();
    }

    private void LoadUserData()
    {
        TextAsset file = Resources.Load<TextAsset>("userData");
        if (file == null)
        {
            Debug.LogError("Не удалось загрузить файл userData");
            return;
        }

        Debug.Log($"Данные из файла userData: {file.text}");

        List<userData> users = JsonConvert.DeserializeObject<List<userData>>(file.text);
        if (users == null || users.Count == 0)
        {
            Debug.LogError("Не удалось десериализовать пользователей из файла userData");
            return;
        }

        Debug.Log($"Загружено {users.Count} пользователей");

        int id = PlayerPrefs.GetInt("id"); // Получаем ID пользователя из PlayerPrefs

        userData user = users.Find(u => u.id == id);
        if (user == null)
        {
            Debug.LogError($"Пользователь с ID {id} не найден");
            return;
        }

        usernameText.text = user.username;
        nameText.text = user.name;
    }
}