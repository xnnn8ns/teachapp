using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelect : MonoBehaviour
{
    [SerializeField]
    private Transform _contentView;

    [SerializeField]
    private GameObject _iconAvatar;

    private Action<int> _actionSelectAvatarID;

    public void FillAvatarScrollList(Action<int> actionSelectedAvatarID)
    {
        _actionSelectAvatarID = actionSelectedAvatarID;
        if (_contentView.childCount > 0)
            return;
        Sprite[] sprites = Resources.LoadAll<Sprite>("Avatars/");
        //Debug.Log(sprites.Length);
        //ClearImagesInScroll();
        foreach (var sprite in sprites)
        {
            GameObject iconObject = Instantiate(_iconAvatar, _contentView);
            iconObject.GetComponent<Image>().sprite = sprite;
            //Debug.Log(sprite.name);
            bool result = int.TryParse(sprite.name, out int avatarID);
            if(result)
                iconObject.GetComponent<Avatar>().SetID(avatarID, ClickAvatarCallBack);
        }
    }

    private void ClearImagesInScroll()
    {
        for (int i = _contentView.childCount - 1; i >= 0; i--)
        {
            Destroy(_contentView.GetChild(i));
        }
    }

    private void ClickAvatarCallBack(int avatarID)
    {
        if (avatarID > 0)
        {
            _actionSelectAvatarID?.Invoke(avatarID);
        }
    }
}
