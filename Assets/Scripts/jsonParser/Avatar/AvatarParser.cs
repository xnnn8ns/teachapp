using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AvatarParser : MonoBehaviour
{
    public Image avatarImage; // Ссылка на компонент Image, который отображает аватарку в топ меню
    public Sprite[] avatars; // Массив всех возможных аватарок
    private string dataPath;

    void Start()
    {
        LoadAvatar(UserData.UserAvatarID); // нужен способ определения id игрока
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
        ComonFunctions.LoadAvatarFromResourceByID(avatarImage, UserData.UserAvatarID);
    }
}
