using System.Collections;
using ResponseJson;
using ResponseTeamJson;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamChangeForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField _enterTeamField;
    [SerializeField] private TMP_Text _teanTitleText;
    [SerializeField] private GameObject _buttonJoin;

    private int enteredTeamID = 0;
    private string enteredTeamTitle = "";
    private int enteredTeamAdminID = 0;

    private void Start()
    {
        UserData.LoadUserData();
        _teanTitleText.text = UserData.CurrentTeamName;
    }

    public void ClickOK()
    {
        UserData.SetCurrentTeam(enteredTeamID, enteredTeamTitle, enteredTeamAdminID);
        StartCoroutine(AddUserToTeam(UserData.UserID, enteredTeamID));
    }

    public void ClickCancel()
    {
        CloseOtherAdditiveScenes();
    }

    public void ClickFindTeam()
    {
        StartCoroutine(CheckDataForUser());
    }

    private IEnumerator CheckDataForUser()
    {
        if (_enterTeamField.text.Length < 1)
        {
            Debug.LogError("Input code with errors");
            yield break;
        }
        StartCoroutine(FindTeamByCode(_enterTeamField.text));
        yield break;
    }

    private IEnumerator FindTeamByCode(string code)
    {
        WWWForm form = new WWWForm();
        form.AddField("code", code);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/get_team_by_code.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            ResponseTeam nativeResponse = ResponseTeam.FromJson(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);
            if (nativeResponse != null && nativeResponse.ResponseCode == 1)
            {
                if (nativeResponse.TeamIDList != null && nativeResponse.TeamIDList.Count > 0)
                {
                    enteredTeamID = nativeResponse.TeamIDList[0].TeamID;
                    enteredTeamTitle = nativeResponse.TeamIDList[0].TeamName;
                    enteredTeamAdminID = nativeResponse.TeamIDList[0].UserIDAdmin;
                    //UserData.SetCurrentTeam(nativeResponse.TeamIDList[0].TeamID, nativeResponse.TeamIDList[0].TeamName, nativeResponse.TeamIDList[0].UserIDAdmin);
                    Debug.Log("Team ID: " + enteredTeamID);
                    Debug.Log("Team Title: " + enteredTeamTitle);
                    _teanTitleText.text = enteredTeamTitle;
                }
            }

        }
    }

    private void CloseOtherAdditiveScenes()
    {
        foreach (var item in SceneManager.GetAllScenes())
        {
            if (item.name == "SelectTeam")
                SceneManager.UnloadSceneAsync(item);
        }
    }


    private IEnumerator AddUserToTeam(int userID, int teamID)
    {
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
            Debug.Log(www.downloadHandler.text);
            //ResponseCode nativeResponse = ResponseCode.FromJson(www.downloadHandler.text);
            //if (nativeResponse.ResponseCodeValue >= 1)
            CloseOtherAdditiveScenes();
            //SceneManager.LoadScene(0);
        }
    }
}
