using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserData : MonoBehaviour
{
    public static string UserID = "";
    public static string UserName = "";
    public static string UserEmail = "";
    public static string UserPassword = "";
    public static int UserAvatarID = 0;
    public static int IsByVK = 0;
    public static int VKID = 0;
    public static int CurrentTeamID = 0;
    public static string CurrentTeamName = "";
    public static int CurrentTeamUserAdminID = 0;
    public static int Score = 0;
    public static string Token = "";

    public static void SetToken(string token)
    {
        Token = token;
        PlayerPrefs.SetString("Token", Token);
    }

    public static void SetUserData(string userID, string userName, string userEmail, string userPassword, int userAvatarID, int isByVK, int vkID, int score)
    {
        UserID = userID;
        UserName = userName;
        UserEmail = userEmail;
        UserPassword = userPassword;
        UserAvatarID = userAvatarID;
        IsByVK = isByVK;
        VKID = vkID;
        Score = score;
        PlayerPrefs.SetString("UserID", UserID);
        PlayerPrefs.SetString("UserName", UserName);
        PlayerPrefs.SetString("UserEmail", UserEmail);
        PlayerPrefs.SetString("UserPassword", UserPassword);
        PlayerPrefs.SetInt("UserAvatarID", UserAvatarID);
        PlayerPrefs.SetInt("IsByVK", IsByVK);
        PlayerPrefs.SetInt("VKID", VKID);
        PlayerPrefs.SetInt("Score", Score);
    }

    public static void LoadUserData()
    {
        UserID = PlayerPrefs.GetString("UserID", "");
        UserName = PlayerPrefs.GetString("UserName", "");
        UserEmail = PlayerPrefs.GetString("UserEmail", "");
        UserPassword = PlayerPrefs.GetString("UserPassword", "");
        UserAvatarID = PlayerPrefs.GetInt("UserAvatarID", 0);
        IsByVK = PlayerPrefs.GetInt("IsByVK", 0);
        VKID = PlayerPrefs.GetInt("VKID", 0);
        CurrentTeamID = PlayerPrefs.GetInt("CurrentTeamID", 0);
        CurrentTeamName = PlayerPrefs.GetString("CurrentTeamName", "");
        CurrentTeamUserAdminID = PlayerPrefs.GetInt("CurrentTeamUserAdminID", 0);
        Score = PlayerPrefs.GetInt("Score", 0);
        Debug.Log("1 Score: " + Score);
    }

    public static bool IsHaveLoginUserData()
    {
        LoadUserData();
        if (UserID != "")
            return true;
        return false;
    }

    public static void SetCurrentTeam(int teamID, string teamName, int userAdminID)
    {
        CurrentTeamID = teamID;
        CurrentTeamName = teamName;
        CurrentTeamUserAdminID = userAdminID;
        PlayerPrefs.SetInt("CurrentTeamID", CurrentTeamID);
        PlayerPrefs.SetString("CurrentTeamName", CurrentTeamName);
        PlayerPrefs.SetInt("CurrentTeamUserAdminID", CurrentTeamUserAdminID);
    }

    public static void SetScore(int score)
    {
        Score = score;
        PlayerPrefs.SetInt("Score", Score);
    }
}
