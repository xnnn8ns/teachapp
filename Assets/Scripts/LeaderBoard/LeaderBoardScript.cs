using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using ResponseUserInTeamJson;

public class UserDataList
{
    public string username;
    public int score;
}

public class LeaderboardData
{
    public string id;
    public string name;
    public int score;
}

public class LeaderBoardScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> obj = new List<GameObject>();
    [SerializeField]
    private GameObject leaderboardItemPrefab;

    [SerializeField]
    private TextMeshProUGUI teamNameText;

    void Start()
    {
        if (UserData.CurrentTeamID == 0)
            UpdateFakeLeaderboardData();
        else
            StartCoroutine(GetUsersInTeam(UserData.CurrentTeamID));
    }

    private IEnumerator GetUsersInTeam(int teamID)
    {
        WWWForm form = new WWWForm();
        form.AddField("teamID", teamID);

        UnityWebRequest www = UnityWebRequest.Post("http://sg12ngec.beget.tech/auth/get_user_list_in_team.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            ResponseUserInTeam nativeResponse = ResponseUserInTeam.FromJson(www.downloadHandler.text);
            if (nativeResponse != null && nativeResponse.ResponseCode == 1)
            {
                if (nativeResponse.UserList != null && nativeResponse.UserList.Count > 0)
                {
                    UserData.LoadUserData();

                    List<LeaderboardData> leaderboardData = new List<LeaderboardData>();
                    int position = 1;
                    foreach (var item in nativeResponse.UserList)
                    {
                        LeaderboardData userData = new LeaderboardData
                        {
                            id = item.UserID,
                            name = item.UserFullName,
                            score = item.Score// userData.score
                        };
                        leaderboardData.Add(userData);
                    }

                    //userName = userData.username;
                    //LeaderboardData userLeaderboardData = new LeaderboardData
                    //{
                    //    id = UserData.UserID,
                    //    name = UserData.UserName,
                    //    score = 0// userData.score
                    //};

                    //leaderboardData.Add(userLeaderboardData);
                    leaderboardData.Sort((a, b) => b.score.CompareTo(a.score));

                    obj.Clear();
                    foreach (LeaderboardData data in leaderboardData)
                    {
                        GameObject leaderboardItem = Instantiate(leaderboardItemPrefab, gameObject.transform);

                        TextMeshProUGUI UserName = leaderboardItem.transform.Find("UserName").GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI UserScore = leaderboardItem.transform.Find("UserScore").GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI Position = leaderboardItem.transform.Find("Position").GetComponent<TextMeshProUGUI>();
                        Image back = leaderboardItem.transform.Find("Backgound").GetComponent<Image>();
                        Image gold = leaderboardItem.transform.Find("gold").GetComponent<Image>();
                        Image silver = leaderboardItem.transform.Find("silver").GetComponent<Image>();
                        Image bronze = leaderboardItem.transform.Find("bronze").GetComponent<Image>();
                        Image avatar = leaderboardItem.transform.Find("avatar").GetComponent<Image>();

                        UserName.text = data.name;
                        UserScore.text = data.score.ToString() + " XP";
                        Position.text = position.ToString();

                        if (data.id != UserData.UserID)
                            back.enabled = false;
                        else
                            ComonFunctions.LoadAvatarFromResourceByID(avatar, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID);

                        if (position != 1)
                        {
                            gold.enabled = false;
                            Position.text = " ";
                        }
                        if (position != 2)
                        {
                            silver.enabled = false;
                            Position.text = " ";
                        }
                        if (position != 3)
                        {
                            bronze.enabled = false;
                            Position.text = " ";
                        }

                        RectTransform rectT = leaderboardItem.GetComponent<RectTransform>();
                        rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 1.1f);
                        obj.Add(leaderboardItem);
                        position++;
                    }

                    teamNameText.text = UserData.CurrentTeamName;
                }
            }
        }
    }

    private void UpdateFakeLeaderboardData()
    {
        int scoreUser = PlayerPrefs.GetInt("AddedScore", 0);
        Debug.Log("UpdateEnemyData: " + scoreUser);
        int position = 1;
        TextAsset[] leaderboardJsonFiles = Resources.LoadAll<TextAsset>("leaderboard");
        if (leaderboardJsonFiles != null && leaderboardJsonFiles.Length > 0)
        {
            TextAsset leaderboardJsonFile = leaderboardJsonFiles[leaderboardJsonFiles.Length - 1];
            List<LeaderboardData> leaderboardData = JsonConvert.DeserializeObject<List<LeaderboardData>>(leaderboardJsonFile.text);
            //TextAsset[] userJsonFiles = Resources.LoadAll<TextAsset>("userdata");
            //if (userJsonFiles != null && userJsonFiles.Length > 0)
            //{
            //TextAsset userJsonFile = userJsonFiles[userJsonFiles.Length - 1];

            //UserDataList userData = JsonConvert.DeserializeObject<UserDataList>(userJsonFile.text);
            UserData.LoadUserData();
            //userName = userData.username;
            LeaderboardData userLeaderboardData = new LeaderboardData
            {
                id = UserData.UserID,
                name = UserData.UserName,
                score = 0// userData.score
            };
            //Debug.Log("UpdateFakeLeaderboardData 1");
            Debug.Log(leaderboardData.Count);
            leaderboardData.Add(userLeaderboardData);
            Debug.Log(leaderboardData.Count);
            //}
            //else
            //{
            //    Debug.LogError("User JSON file not found in Resources folder.");
            //}
            leaderboardData.Sort((a, b) => b.score.CompareTo(a.score));

            obj.Clear();
            foreach (LeaderboardData data in leaderboardData)
            {
                GameObject leaderboardItem = Instantiate(leaderboardItemPrefab, gameObject.transform);

                TextMeshProUGUI UserName = leaderboardItem.transform.Find("UserName").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI UserScore = leaderboardItem.transform.Find("UserScore").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI Position = leaderboardItem.transform.Find("Position").GetComponent<TextMeshProUGUI>();
                Image back = leaderboardItem.transform.Find("Backgound").GetComponent<Image>();
                Image gold = leaderboardItem.transform.Find("gold").GetComponent<Image>();
                Image silver = leaderboardItem.transform.Find("silver").GetComponent<Image>();
                Image bronze = leaderboardItem.transform.Find("bronze").GetComponent<Image>();
                Image avatar = leaderboardItem.transform.Find("avatar").GetComponent<Image>();

                UserName.text = data.name;
                UserScore.text = data.score.ToString() + " XP";
                Position.text = position.ToString();

                if (data.id != UserData.UserID)
                    back.enabled = false;
                else
                    ComonFunctions.LoadAvatarFromResourceByID(avatar, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID);

                if (position != 1)
                {
                    gold.enabled = false;
                    Position.text = " ";
                }
                if (position != 2)
                {
                    silver.enabled = false;
                    Position.text = " ";
                }
                if (position != 3)
                {
                    bronze.enabled = false;
                    Position.text = " ";
                }

                RectTransform rectT = leaderboardItem.GetComponent<RectTransform>();
                rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 1.1f);
                obj.Add(leaderboardItem);
                position++;
            }
        }
        else
        {
            Debug.LogError("Leaderboard JSON file not found in Resources folder.");
        }
    }

    private void SaveEnemyScore(string _id, int _score)
    {
        string jsonLeaderBoardPath = "/leaderboard.json";

        if (!File.Exists(Application.persistentDataPath + jsonLeaderBoardPath))
        {
            FileStream fs = File.Create(Application.persistentDataPath + jsonLeaderBoardPath);
            fs.Dispose();
            TextAsset txt = (TextAsset)Resources.Load("leaderboard", typeof(TextAsset));
            string jsonTemp = txt.text;
            File.WriteAllText(Application.persistentDataPath + jsonLeaderBoardPath, jsonTemp);
        }

        string json = File.ReadAllText(Application.persistentDataPath + jsonLeaderBoardPath);
        List<LeaderboardData> LeaderboardDataList = new List<LeaderboardData>();

        LeaderboardDataList = JsonConvert.DeserializeObject<List<LeaderboardData>>(json);


        LeaderboardData leaderboardData = LeaderboardDataList.Find(item => item.id == _id);

        if (leaderboardData != null)
        {

            leaderboardData.score = _score;
            leaderboardData.id = _id;
            json = JsonConvert.SerializeObject(LeaderboardDataList, Formatting.Indented);

            File.WriteAllText(Application.persistentDataPath + jsonLeaderBoardPath, json);
        }
        else
        {
            Debug.LogError("enemy data with id " + _id + " not found!");
        }
    }
}
