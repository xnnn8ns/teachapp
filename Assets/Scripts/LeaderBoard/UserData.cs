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
    public static int IsByVK = 0;
    public static int VKID = 0;

    public static void SetUserData(int userID, string userName, string userEmail, string userPassword, int userAvatarID, int isByVK, int vkID)
    {
        UserID = userID;
        UserName = userName;
        UserEmail = userEmail;
        UserPassword = userPassword;
        UserAvatarID = userAvatarID;
        IsByVK = isByVK;
        VKID = vkID;
        PlayerPrefs.SetInt("UserID", UserID);
        PlayerPrefs.SetString("UserName", UserName);
        PlayerPrefs.SetString("UserEmail", UserEmail);
        PlayerPrefs.SetString("UserPassword", UserPassword);
        PlayerPrefs.SetInt("UserAvatarID", UserAvatarID);
        PlayerPrefs.SetInt("IsByVK", IsByVK);
        PlayerPrefs.SetInt("VKID", VKID);
    }

    public static void LoadUserData()
    {
        UserID = PlayerPrefs.GetInt("UserID", 0);
        UserName = PlayerPrefs.GetString("UserName", "");
        UserEmail = PlayerPrefs.GetString("UserEmail", "");
        UserPassword = PlayerPrefs.GetString("UserPassword", "");
        UserAvatarID = PlayerPrefs.GetInt("UserAvatarID", 0);
        IsByVK = PlayerPrefs.GetInt("IsByVK", 0);
        VKID = PlayerPrefs.GetInt("VKID", 0);
    }

    public static bool IsHaveLoginUserData()
    {
        LoadUserData();
        if (UserID > 0)
            return true;
        return false;
    }
}
