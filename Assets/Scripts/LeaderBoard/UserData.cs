using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserData : MonoBehaviour
{
    public static int UserID = 0;
    public static string UserName = "";
    public static string UserEmail = "";
    public static string UserPassword = "";
    public static int UserAvatarID = 0;

    public static void SetUserData(int userID, string userName, string userEmail, string userPassword, int userAvatarID)
    {
        UserID = userID;
        UserName = userName;
        UserEmail = userEmail;
        UserPassword = userPassword;
        UserAvatarID = userAvatarID;
        PlayerPrefs.SetInt("UserID", UserID);
        PlayerPrefs.SetString("UserName", UserName);
        PlayerPrefs.SetString("UserEmail", UserEmail);
        PlayerPrefs.SetString("UserPassword", UserPassword);
        PlayerPrefs.SetInt("UserAvatarID", UserAvatarID);
    }

    public static void LoadUserData()
    {
        UserID = PlayerPrefs.GetInt("UserID", 0);
        UserName = PlayerPrefs.GetString("UserName", "");
        UserEmail = PlayerPrefs.GetString("UserEmail", "");
        UserPassword = PlayerPrefs.GetString("UserPassword", "");
        UserAvatarID = PlayerPrefs.GetInt("UserAvatarID", 0);
    }
}
