using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using System.Linq;

public class NameDataLoad : MonoBehaviour
{
    [System.Serializable]
    public class UserData
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

        UserData[] users = JsonConvert.DeserializeObject<UserData[]>(file.text);
        if (users == null || users.Length == 0)
        {
            Debug.LogError("Не удалось десериализовать пользователей из файла userData");
            return;
        }

        int id = PlayerPrefs.GetInt("id"); // Получаем ID пользователя из PlayerPrefs
        Debug.Log($"Поиск пользователя с ID {id}");

        UserData user = users.FirstOrDefault(u => u.id == id);
        if (user == null)
        {
            Debug.LogError($"Пользователь с ID {id} не найден");
            return;
        }

        Debug.Log($"Найден пользователь: {user.username}");
        usernameText.text = user.username;
        nameText.text = user.name;
    }
}