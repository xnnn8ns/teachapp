using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataController : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvasUserData;
    [SerializeField]
    private GameObject _canvasSelectAvatar;

    public void ClickToSelectAvatar()
    {
        _canvasUserData.SetActive(false);
        _canvasSelectAvatar.GetComponent<AvatarSelect>().FillAvatarScrollList(ClickSelectedAvatar);
        _canvasSelectAvatar.SetActive(true);
    }

    private void ClickSelectedAvatar(int avatarID)
    {
        _canvasUserData.SetActive(true);
        _canvasUserData.GetComponent<AuthForm>().SetAvatarImageByID(avatarID);
        _canvasSelectAvatar.SetActive(false);
    }

    public void ClickCancelSelectAvatar()
    {
        _canvasUserData.SetActive(true);
        _canvasSelectAvatar.SetActive(false);
    }
}
