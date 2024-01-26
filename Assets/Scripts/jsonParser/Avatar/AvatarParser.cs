using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvatarParser : MonoBehaviour
{
    public Image avatarImage; // Ссылка на компонент Image, который отображает аватарку в топ меню
    public Sprite[] avatars; // Массив всех возможных аватарок
    private string dataPath;

    void Start()
    {
        LoadAvatar(UserData.UserAvatarID); // нужен способ определения id игрока
        AvatarSelector.OnAvatarChanged += OnAvatarChanged; // Подписываемся на событие
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        AvatarSelector.OnAvatarChanged -= OnAvatarChanged; // Отписываемся от события
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnAvatarChanged(int index)
    {
        avatarImage.sprite = avatars[index];
    }
    void LoadAvatar(int id)
    {
        ComonFunctions.LoadAvatarFromResourceByID(avatarImage, UserData.UserAvatarID);
    }

    void OnSceneUnloaded(Scene scene)
    {
        // Если выгружена любая сцена и остались только MapScene и DontDestroyOnLoad
        if (SceneManager.sceneCount == 2)
        {
            LoadAvatar(UserData.UserAvatarID);
        }
    }
}
