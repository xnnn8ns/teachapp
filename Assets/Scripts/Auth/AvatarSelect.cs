using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        if (UserData.VKID > 0)
        {
            Sprite spriteVK = ComonFunctions.GetSpriteFromResourceByVKID(UserData.VKID);
            if(spriteVK)
                sprites = sprites.Concat(new Sprite[] { spriteVK }).ToArray();
        }
        foreach (var sprite in sprites)
        {
            GameObject iconObject = Instantiate(_iconAvatar, _contentView);
            iconObject.GetComponent<Image>().sprite = sprite;
            bool result = int.TryParse(sprite.name, out int avatarID);
            if(result)
                iconObject.GetComponent<Avatar>().SetID(avatarID, ClickAvatarCallBack);
            else if (UserData.VKID > 0)
                iconObject.GetComponent<Avatar>().SetID(0, ClickAvatarCallBack);

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
        _actionSelectAvatarID?.Invoke(avatarID);
    }
}
