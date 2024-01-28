using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using ResponseUserInTeamJson;
using System;
using System.Linq;

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
    public string icon;
}

public class LeaderBoardScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> obj = new List<GameObject>();
    [SerializeField]
    private GameObject leaderboardItemPrefab;

    [SerializeField]
    private TextMeshProUGUI teamNameText;
    private int lastUserPosition = -1;
    public bool userMovedUp { get; private set; }
    public bool userMovedDown { get; private set; }

    private void Start()
    {
        if (UserData.CurrentTeamID == 0)
            GetUserLeaderBoardList();// UpdateFakeLeaderboardData();
        else
            StartCoroutine(GetUsersInTeam(UserData.CurrentTeamID));

        if (IsNeedAddDayScoreForFakeUsers())
            AddDayScoreForFakeUsers();

        Get30RandomUserList();
        //GetRandomAvatar();
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
                        UserScore.text = data.score.ToString();// + " XP";
                        Position.text = position.ToString();

                        if (data.id != UserData.UserID)
                            back.enabled = false;
                        else
                            ComonFunctions.LoadAvatarFromResourceByID(avatar, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID);

                        if (position != 1)
                        {
                            gold.enabled = false;
                            //Position.text = " ";
                        }
                        if (position != 2)
                        {
                            silver.enabled = false;
                            //Position.text = " ";
                        }
                        if (position != 3)
                        {
                            bronze.enabled = false;
                            //Position.text = " ";
                        }

                        //RectTransform rectT = leaderboardItem.GetComponent<RectTransform>();
                        //rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 1.1f);
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
                score = UserData.Score
            };
            if (userLeaderboardData.name == "")
                userLeaderboardData.name = LangAsset.GetValueByKey("UserName");
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
                UserScore.text = data.score.ToString();// + " XP";
                Position.text = position.ToString();

                if (data.id != UserData.UserID)
                    back.enabled = false;
                else
                    ComonFunctions.LoadAvatarFromResourceByID(avatar, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID);

                if (position != 1)
                {
                    gold.enabled = false;
                    //Position.text = " ";
                }
                if (position != 2)
                {
                    silver.enabled = false;
                    //Position.text = " ";
                }
                if (position != 3)
                {
                    bronze.enabled = false;
                    //Position.text = " ";
                }

                //RectTransform rectT = leaderboardItem.GetComponent<RectTransform>();
                //rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 1.1f);
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

    private static List<string> Get30RandomUserList()
    {
        List<string> _userList = new List<string>();
        System.Random rand = new System.Random();
        TextAsset inn_name = Resources.Load<TextAsset>("names_txt/500_inn_names");
        string[] names = inn_name.text.Split('\n');
        for (int i = 0; i < 3; i++)
            _userList.Add(names[rand.Next(0, names.Length - 1)]);

        inn_name = Resources.Load<TextAsset>("names_txt/500_village_names");
        names = inn_name.text.Split('\n');
        for (int i = 0; i < 4; i++)
            _userList.Add(names[rand.Next(0, names.Length - 1)]);

        inn_name = Resources.Load<TextAsset>("names_txt/11000_general_names");
        names = inn_name.text.Split('\n');
        for (int i = 0; i < 4; i++)
            _userList.Add(names[rand.Next(0, names.Length - 1)]);

        inn_name = Resources.Load<TextAsset>("names_txt/name_english");
        names = inn_name.text.Split('\n');
        for (int i = 0; i < 6; i++)
        {
            string n = names[rand.Next(0, names.Length - 1)].ToLower();
            n = n.First().ToString().ToUpper() + String.Join("", n.Skip(1));
            _userList.Add(n);
        }

        TextAsset sur_names = Resources.Load<TextAsset>("names_txt/surname_english");
        string[] surnames = inn_name.text.Split('\n');
        for (int i = 0; i < 12; i++)
        {
            string n = names[rand.Next(0, names.Length - 1)].ToLower();
            n = n.First().ToString().ToUpper() + String.Join("", n.Skip(1));
            string s = surnames[rand.Next(0, surnames.Length - 1)].ToLower();
            s = s.First().ToString().ToUpper() + String.Join("", s.Skip(1));

            _userList.Add(n + " " + s);
        }
        //foreach (var item in _userList)
        //{
        //    Debug.Log(item);
        //}
        return _userList;
    }

    private static List<LeaderboardData> GetFakeJSONForStartLeaderBoard()
    {
        List<string> userList = Get30RandomUserList();
        List<LeaderboardData> leaderboardData = new List<LeaderboardData>();

        int count = 1;
        System.Random rand = new System.Random();
        foreach (var item in userList)
        {
            LeaderboardData userData = new LeaderboardData
            {
                id = count.ToString(),
                name = item,
                icon = GetRandomAvatar(),
                score = rand.Next(0, 25) * rand.Next(0, 4)
            };

            leaderboardData.Add(userData);

            count++;
        }

        return leaderboardData;
    }

    private static string GetRandomAvatar()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Avatars/");
        System.Random rand = new System.Random();
        Sprite s = sprites[rand.Next(0, sprites.Length)];
        //Debug.Log(s.name);
        return s.name;
    }

    private void GetUserLeaderBoardList()
    {
        List<LeaderboardData> leaderboardData = GetOrCreateFileLeaderBoard(Settings.jsonLeaderBoardFilePath);

        int scoreUser = PlayerPrefs.GetInt("AddedScore", 0);
        //Debug.Log("UpdateEnemyData: " + scoreUser);
        int position = 1;

        UserData.LoadUserData();
        LeaderboardData userLeaderboardData = new LeaderboardData
        {
            id = UserData.UserID,
            name = UserData.UserName,
            score = UserData.Score
        };
        if (userLeaderboardData.name == "")
            userLeaderboardData.name = LangAsset.GetValueByKey("UserName");

        leaderboardData.Add(userLeaderboardData);

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
            UserScore.text = data.score.ToString();// + " XP";
            Position.text = position.ToString();

            if (data.id != UserData.UserID)
            {
                back.enabled = false;
                avatar.sprite = Resources.Load<Sprite>("Avatars/" + data.icon);
            }
            else
                ComonFunctions.LoadAvatarFromResourceByID(avatar, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID);
            
            // сравниваем прогресс пользователя
            if (data.id == UserData.UserID)
            {
                if (PlayerPrefs.HasKey("LastUserPosition"))
                {
                    lastUserPosition = PlayerPrefs.GetInt("LastUserPosition");
                }
                else
                {
                    lastUserPosition = int.MaxValue;
                }

                int newUserPosition = position;

                Debug.Log("Current position: " + newUserPosition);
                Debug.Log("Last saved position: " + lastUserPosition);

                if (newUserPosition < lastUserPosition)
                {
                    Debug.Log("Поздравляем! Вы поднялись на " + lastUserPosition + " место!");
                    userMovedUp = true;
                }
                else if (newUserPosition > lastUserPosition)
                {
                    Debug.Log("Вы опустились на " + (newUserPosition - lastUserPosition) + " место!");
                    userMovedDown = true;
                }
                else if (newUserPosition == lastUserPosition)
                {
                    Debug.Log("Вы не сдвинулись с места!");
                }

                lastUserPosition = newUserPosition;
                PlayerPrefs.SetInt("LastUserPosition", lastUserPosition);
                PlayerPrefs.Save();
                StartCoroutine(SetScrollPosition(position));
            }

            if (position != 1)
                gold.enabled = false;
            if (position != 2)
                silver.enabled = false;
            if (position != 3)
                bronze.enabled = false;

            obj.Add(leaderboardItem);
            position++;
        }
    }

    // метод для установки позиции скролла так, чтобы пользователь видел себя по середине экрана
    IEnumerator SetScrollPosition(int position)
    {
        // Ждем следующего кадра
        yield return null;

        ScrollRect scrollRect = this.transform.parent.parent.GetComponent<ScrollRect>();
        float itemHeight = this.transform.GetChild(0).GetComponent<RectTransform>().rect.height; // Высота каждого элемента
        float halfVisibleHeight = scrollRect.GetComponent<RectTransform>().rect.height / 2; // Половина высоты видимой области
        float startPosition = scrollRect.content.localPosition.y; // Начальная позиция
        float desiredPosition = startPosition + (itemHeight * position) - halfVisibleHeight;
        scrollRect.content.localPosition = new Vector2(scrollRect.content.localPosition.x, desiredPosition);
    }

    private List<LeaderboardData> GetOrCreateFileLeaderBoard(string filePathSave)
    {
        List<LeaderboardData> userList = new List<LeaderboardData>();

        if (File.Exists(Application.persistentDataPath + filePathSave))
        {
            return JsonConvert.DeserializeObject<List<LeaderboardData>>(File.ReadAllText(Application.persistentDataPath + filePathSave));
        }
        else
        {
            FileStream fs = File.Create(Application.persistentDataPath + filePathSave);
            fs.Dispose();

            List<LeaderboardData> fakeUserList = GetFakeJSONForStartLeaderBoard();

            SaveLeaderBoard(filePathSave, fakeUserList);

            return fakeUserList;
        }
    }

    private void SaveLeaderBoard(string filePathSave, List<LeaderboardData> leaderboardData)
    {
        string json = JsonConvert.SerializeObject(leaderboardData);
        File.WriteAllText(Application.persistentDataPath + filePathSave, json);
        Debug.Log(json);
    }

    private void AddDayScoreForFakeUsers()
    {
        List<LeaderboardData> leaderboardData = GetOrCreateFileLeaderBoard(Settings.jsonLeaderBoardFilePath);
        leaderboardData.Sort((a, b) => b.score.CompareTo(a.score));
        System.Random rand = new System.Random();
        for (int i = leaderboardData.Count - 1; i >= 0; i--)
        {
            int delta = 0;
            if (i > 25)
                delta = i*rand.Next(1, 3) + rand.Next(5, 20);
            else if (i > 10)
                delta = i*rand.Next(0, 2) + rand.Next(5, 15);
            else
                delta = i*rand.Next(0, 2) + rand.Next(3, 10);

            leaderboardData[i].score += delta;
            
            //Debug.Log(i * rand.Next(0, 4));
            //Debug.Log(leaderboardData[i].score);
        }
        SaveLeaderBoard(Settings.jsonLeaderBoardFilePath, leaderboardData);
        PlayerPrefs.SetString("LastTimeAddDayScore", DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
    }

    private bool IsNeedAddDayScoreForFakeUsers()
    {
        //Debug.Log(DateTimeOffset.Now.ToUnixTimeSeconds());
        //Debug.Log(long.Parse(PlayerPrefs.GetString("LastTimeAddDayScore", "0")));
        long now = DateTimeOffset.Now.ToUnixTimeSeconds();
        long last = long.Parse(PlayerPrefs.GetString("LastTimeAddDayScore", "0"));
        Debug.Log(now - last);
        long day = 60 * 60 * 12;
        Debug.Log((now - last) > day);
        if ((now - last) > day)
            return true;
        else
            return false;
    }
}
