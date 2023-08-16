using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ComonFunctions : MonoBehaviour
{
    public static ComonFunctions Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }

    public IEnumerator GetIconFromURLByUserID(string userID, string url, Image image)
    {
        if (userID.Length == 0 || image == null)
            yield break;

        string link = Application.persistentDataPath + "/" + userID + "_" + 50 + ".png";

        if (!File.Exists(link))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            Debug.Log(www.url);
            www.timeout = 5;
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.result);
                Debug.Log(www.responseCode);
                Debug.Log(www.error);
            }
            else
            {
                Texture2D myTexture = (Texture2D)((DownloadHandlerTexture)www.downloadHandler).texture;
                Sprite mySprite = Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                if (mySprite != null)
                {
                    byte[] textureBytes = mySprite.texture.EncodeToPNG();
                    File.WriteAllBytes(link, textureBytes);
                }
                Debug.Log(mySprite.border);
                image.sprite = mySprite;
                image.enabled = true;
            }
        }
        else
        {
            Sprite mySprite = LoadSpriteFromMemory(link);
            image.sprite = mySprite;
            image.enabled = true;
        }
    }

    private Sprite LoadSpriteFromMemory(string link)
    {
        byte[] textureBytes = File.ReadAllBytes(link);
        Texture2D loadedTexture = new Texture2D(0, 0);
        loadedTexture.LoadImage(textureBytes);
        return Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
    }

    public static void LoadAvatarFromResourceByID(Image imageAvatar, int avatarID, int isByVK = 0, int VKID = 0)
    {

        if (avatarID == 0)
        {
            if (isByVK == 0)
                imageAvatar.sprite = null;
            else
            {
                Sprite sprite = GetSpriteFromResourceByVKID(VKID);
                if(sprite)
                    imageAvatar.sprite = sprite;
            }
            return;
        }
        imageAvatar.sprite = Resources.Load<Sprite>("Avatars/" + avatarID.ToString());
    }

    public static Sprite GetSpriteFromResourceByVKID(int VKID)
    {
        Sprite sprite = null;
        string link = Application.persistentDataPath + "/" + VKID + "_" + 50 + ".png";
        if (File.Exists(link))
        {
            byte[] textureBytes = File.ReadAllBytes(link);
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(textureBytes);
            sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
        }

        return sprite;
    }

}
