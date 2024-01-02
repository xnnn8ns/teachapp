using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AvatarParser : MonoBehaviour
{
    public Image avatarImage; // Ссылка на компонент Image, который отображает аватарку в топ меню
    public Sprite[] avatars; // Массив всех возможных аватарок
    private string dataPath;

    [System.Serializable]
    public class UserData
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private int avatar;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
    }

    void Start()
    {
        dataPath = Path.Combine(Application.dataPath, "Resources/userAvatar.json");
        LoadAvatar(1); // нужен способ определения id игрока
        AvatarSelector.OnAvatarChanged += OnAvatarChanged; // Подписываемся на событие
    }

    void OnDestroy()
    {
        AvatarSelector.OnAvatarChanged -= OnAvatarChanged; // Отписываемся от события
    }

    void OnAvatarChanged(int index)
    {
        avatarImage.sprite = avatars[index];
    }
    void LoadAvatar(int id)
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            UserData data = JsonUtility.FromJson<UserData>(json);
            if (data.ID == id)
            {
                avatarImage.sprite = avatars[data.Avatar];
            }
        }
    }
}
