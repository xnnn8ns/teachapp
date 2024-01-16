using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameDataLoad : MonoBehaviour
{
    [SerializeField]
    private TMP_Text usernameText;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Image imageAvatar;
    [SerializeField]
    private Image imagePlusAvatar;
    [SerializeField]
    private GameObject flagParent;
    [SerializeField]
    private GameObject settengsButton;


    private void Start()
    {
        LoadUserData();
        flagParent.SetActive(false);
    }

    private void LoadUserData()
    {
        usernameText.text = UserData.UserName;
        nameText.text = UserData.UserFullName;

        if (UserData.UserID.Length > 0 && int.Parse(UserData.UserID) > 0) {
            imagePlusAvatar.gameObject.SetActive(false);
            ComonFunctions.LoadAvatarFromResourceByID(imageAvatar, UserData.UserAvatarID);
            settengsButton.SetActive(true);
        }
        else
        {
            imagePlusAvatar.gameObject.SetActive(true);
            settengsButton.SetActive(false);
        }
    }

    public void ClickFlag()
    {
        flagParent.SetActive(!flagParent.activeSelf);
        if (flagParent.activeSelf)
            flagParent.GetComponent<ChangeLanguage>()?.LoadFlags();
    }

    public void ClickAcount()
    {
        if (UserData.UserID.Length > 0 && int.Parse(UserData.UserID) > 0)
        {
            SceneManager.LoadScene("WindowSelectIconScene", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("RegistrationScene", LoadSceneMode.Additive);
        }
    }
}