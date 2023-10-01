using System.Collections;
using System.Collections.Generic;
using System.IO;
using ResponseTeamJson;
using ResponseJson;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ComonFunctions : MonoBehaviour
{
    public static ComonFunctions Instance;

    private void Awake()
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
                    StartCoroutine(UploadIconVKToServer(userID.ToString(), textureBytes));
                }
                //Debug.Log(mySprite.border);
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

    private IEnumerator UploadIconVKToServer(string userID, byte[] textureBytes)
    {
        WWWForm form = new WWWForm();
        string fileName = userID + ".png";
        //form.AddBinaryData(fileName, textureBytes);
        form.AddBinaryData("userImage", textureBytes, fileName, "image/png");

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/upload_vk_icon.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {  
            ResponseCode nativeResponse = ResponseCode.FromJson(www.downloadHandler.text);
            Debug.Log("Form upload complete! " + nativeResponse.ResponseCodeValue);
            Debug.Log("Form upload complete! " + nativeResponse.ResponseData);

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
            {
                imageAvatar.sprite = Resources.Load<Sprite>("Avatars/1");
                //imageAvatar.sprite = null;
            }
            else
            {
                Sprite sprite = GetSpriteFromResourceByVKID(VKID);
                if (sprite)
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

    public IEnumerator GetUserTeamID(string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        
        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/get_user_team_id.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(userID);
            Debug.Log(www.downloadHandler.text);
            ResponseTeam nativeResponse = ResponseTeam.FromJson(www.downloadHandler.text);
            Debug.Log(nativeResponse);
            if (nativeResponse != null && nativeResponse.ResponseCode == 1)
            {
                if (nativeResponse.TeamIDList != null && nativeResponse.TeamIDList.Count > 0)
                {
                    UserData.SetCurrentTeam(nativeResponse.TeamIDList[0].TeamID, nativeResponse.TeamIDList[0].TeamName, nativeResponse.TeamIDList[0].UserIDAdmin);
                    Debug.Log("Team ID: " + UserData.CurrentTeamID);
                    Debug.Log("Team ID: " + UserData.CurrentTeamName);
                }
            }
        }
    }

    public IEnumerator UpdateUser(string userID, string name, string email, string password, int avatarID, int isByVK, int VKID, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", userID);
        form.AddField("fullName", name);
        form.AddField("userEmail", email);
        form.AddField("password", password);
        form.AddField("avatarID", avatarID);
        form.AddField("isByVK", isByVK);
        form.AddField("VKID", VKID);
        form.AddField("score", score);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/update_user.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
            UserData.SetUserData(userID, name, email, password, avatarID, isByVK, VKID, score);
        }
    }

    //public IEnumerator AddUserToTeam(int userID, int teamID)
    //{
    //    Debug.Log("CreateNewUser: start");
    //    WWWForm form = new WWWForm();
    //    form.AddField("userID", userID);
    //    form.AddField("teamID", teamID);

    //    UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/add_user_to_team.php", form);
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Debug.Log("Form upload complete!");
    //        ResponseCode nativeResponse = ResponseCode.FromJson(www.downloadHandler.text);
    //        if (nativeResponse.ResponseCodeValue >= 1)
    //            SceneManager.LoadScene(0);
    //    }
    //}

    public static float GetScaleForShelf()
    {
        float currentScreenCoefProportion = (float)Screen.height / Screen.width;
        float defaultScreenCoefProportion = 2.164251f;// for iPhone11 // need scale 5
        float scale = defaultScreenCoefProportion - 1.778667f; // 1.778667 for 6 // need scale 5

        float deltaFromDefault = currentScreenCoefProportion / defaultScreenCoefProportion;

        float targetScale = 5f / deltaFromDefault;

        Debug.Log(deltaFromDefault);
        Debug.Log(targetScale);
        return targetScale;
    }
}

