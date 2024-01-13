using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private Image imageAvatar;
    [SerializeField]
    private Image imagePlusAvatar;


    private void Start()
    {
        LoadUserData();
    }

    private void LoadUserData()
    {
        usernameText.text = UserData.UserName;
        nameText.text = UserData.UserFullName;

        if (UserData.UserAvatarID > 0) {
            imagePlusAvatar.enabled = false;
            ComonFunctions.LoadAvatarFromResourceByID(imageAvatar, UserData.UserAvatarID);
        }
        else
        {
            imagePlusAvatar.enabled = true;
        }
    }
}