using System.Collections;
using ResponseJson;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ProcessDeepLinkManager : MonoBehaviour
{
    public static ProcessDeepLinkManager Instance { get; private set; }
    //public string deeplinkURL;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                onDeepLinkActivated(Application.absoluteURL);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void onDeepLinkActivated(string url)
    {
        //Debug.Log(url);
        //deeplinkURL = url;
        string team = url.Split('?')[1];
        //Debug.Log("Group: " + group);
        OpenShareLink(team);
    }

    private void OpenShareLink(string obj)
    {
        //string uid = obj.Split("="[0])[1];
        if (int.TryParse(obj, out int result))
        {
            PlayerPrefs.SetInt("CurrentTeamID", result);
            UserData.LoadUserData();
            //Debug.Log("UserID: " + UserData.UserID);
            if (UserData.UserID != "")
            {
                StartCoroutine(AddUserToTeam(UserData.UserID, result));
            }
        }
        
    }

    private IEnumerator AddUserToTeam(string userID, int teamID)
    {
        Debug.Log("CreateNewUser: start");
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        form.AddField("teamID", teamID);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/add_user_to_team.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            ResponseCode nativeResponse = ResponseCode.FromJson(www.downloadHandler.text);
            if (nativeResponse.ResponseCodeValue >= 1)
                SceneManager.LoadScene(0);
        }
    }
}
