using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Linq;


public class ListIconScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> obj = new List<GameObject>();
    [SerializeField]
    private GameObject iconItemPrefab;

    [SerializeField]
    private TextMeshProUGUI titleText;

    private int _selectedAvatarID = 0;

    private void Start()
    {
        titleText.text = LangAsset.GetValueByKey("SelectIcon");
        GetUserLeaderBoardList();
    }

    private void GetUserLeaderBoardList()
    {
        

        Sprite[] sprites = Resources.LoadAll<Sprite>("Avatars/");
        UserData.LoadUserData();
        _selectedAvatarID = UserData.UserAvatarID;
        obj.Clear();
        foreach (Sprite data in sprites)
        {
            GameObject iconItem = Instantiate(iconItemPrefab, gameObject.transform);

            Image back = iconItem.transform.Find("Backgound").GetComponent<Image>();
            Image avatar = iconItem.transform.Find("avatar").GetComponent<Image>();

            int.TryParse(data.name, out int avatarID);

            if (avatarID != UserData.UserAvatarID)
                back.enabled = false;
            else
                back.enabled = true;

            avatar.sprite = data;
            //Debug.Log(avatarID);
            avatar.GetComponent<Avatar>().SetID(avatarID, ClickAvatarCallBack);
            obj.Add(iconItem);
        }
    }

    private void ClickAvatarCallBack(int iconID)
    {
        _selectedAvatarID = iconID;
        Debug.Log("ClickAvatarCallBack");
        Debug.Log(_selectedAvatarID);
        foreach (var iconItem in obj)
        {
            Image back = iconItem.transform.Find("Backgound").GetComponent<Image>();
            Image avatar = iconItem.transform.Find("avatar").GetComponent<Image>();

            int.TryParse(avatar.sprite.name, out int avatarID);
            //Debug.Log(avatar.sprite.name);
            if (_selectedAvatarID != avatarID)
                back.enabled = false;
            else
                back.enabled = true;
        }
    }

    public void ClickOK()
    {
        UserData.UserAvatarID = _selectedAvatarID;
        UserData.SetAvatar();
    }
}
