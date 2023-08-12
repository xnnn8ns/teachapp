using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;


public class UserDataList
{
    public string username;
    public int score;
}

public class LeaderboardData
{
    public int id;
    public string name;
    public int score;
}

public class LeaderBoardScript : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private int score;
    private bool buttonPressed = false;
    [SerializeField] private List<GameObject> obj = new List<GameObject>();
    [SerializeField] private GameObject leaderboardItemPrefab;

    private string TempCurrentUserName = "";

    private string userName;
    void Start()
    {
        UpdateEnemyData();
    }

    private void UpdateEnemyData()
    {
        int scoreUser = PlayerPrefs.GetInt("AddedScore", 0);
        Debug.Log("UpdateEnemyData: " + scoreUser);
        int position = 1;
        TextAsset[] leaderboardJsonFiles = Resources.LoadAll<TextAsset>("leaderboard");
        if (leaderboardJsonFiles != null && leaderboardJsonFiles.Length > 0)
        {
            TextAsset leaderboardJsonFile = leaderboardJsonFiles[leaderboardJsonFiles.Length - 1];
            List<LeaderboardData> leaderboardData = JsonConvert.DeserializeObject<List<LeaderboardData>>(leaderboardJsonFile.text);
            TextAsset[] userJsonFiles = Resources.LoadAll<TextAsset>("userdata");
            if (userJsonFiles != null && userJsonFiles.Length > 0)
            {
                TextAsset userJsonFile = userJsonFiles[userJsonFiles.Length - 1];

                UserDataList userData = JsonConvert.DeserializeObject<UserDataList>(userJsonFile.text);
                userName = userData.username;
                LeaderboardData userLeaderboardData = new LeaderboardData
                {
                    name = userData.username,
                    score = scoreUser// userData.score
                };

                leaderboardData.Add(userLeaderboardData);
            }
            else
            {
                Debug.LogError("User JSON file not found in Resources folder.");
            }
            leaderboardData.Sort((a, b) => b.score.CompareTo(a.score));

            obj.Clear();
            foreach (LeaderboardData data in leaderboardData)
            {
                GameObject leaderboardItem = Instantiate(leaderboardItemPrefab,gameObject.transform);

                TextMeshProUGUI UserName = leaderboardItem.transform.Find("UserName").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI UserScore = leaderboardItem.transform.Find("UserScore").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI Position = leaderboardItem.transform.Find("Position").GetComponent<TextMeshProUGUI>();
                Image back = leaderboardItem.transform.Find("Backgound").GetComponent<Image>();
                Image gold = leaderboardItem.transform.Find("gold").GetComponent<Image>();
                Image silver = leaderboardItem.transform.Find("silver").GetComponent<Image>();
                Image bronze = leaderboardItem.transform.Find("bronze").GetComponent<Image>();

                UserName.text = data.name;
                UserScore.text = data.score.ToString()+" XP";
                Position.text = position.ToString();

                if (data.name != userName)                    
                { 
                    back.enabled = false;
                }
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
                rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width*1.1f);
                obj.Add(leaderboardItem);
                position++;
            }
        }
        else
        {
            Debug.LogError("Leaderboard JSON file not found in Resources folder.");
        }
    }



     public void SaveEnemyScore(int _id, int _score)
    {
        string jsonFilePath = "/Resources/leaderboard.json";
        string json = File.ReadAllText(Application.dataPath + jsonFilePath);
        List<LeaderboardData> LeaderboardDataList = new List<LeaderboardData>();

        LeaderboardDataList = JsonConvert.DeserializeObject<List<LeaderboardData>>(json);


        LeaderboardData leaderboardData = LeaderboardDataList.Find(item => item.id == _id);

        if (leaderboardData != null)
        {

            leaderboardData.score = _score;
            leaderboardData.id = _id;
            json = JsonConvert.SerializeObject(LeaderboardDataList, Formatting.Indented);

            File.WriteAllText(Application.dataPath + jsonFilePath, json);
        }
        else
        {
            Debug.LogError("enemy data with id " + _id + " not found!");
        }
    }
}
