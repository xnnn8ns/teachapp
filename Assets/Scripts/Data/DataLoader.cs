using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;
using ResponseTaskJSON;
using Unity.VisualScripting;

public class DataLoader : MonoBehaviour
{
    private const string apiUrl = "http://45.12.239.30:8000/api/tasks/";

    private void Start()
    {
        GetTaskData();
        //GetFromJSON();
        //UserData.LoadUserData();
        //SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
        //StartCoroutine(ComonFunctions.Instance.GetUserTeamID(UserData.UserID));
    }

    private void GetFromJSON()
    {
        
        Settings.Current_Level = DataLoader.GetCurrentLevel();
        string strJSON;
        strJSON = Resources.Load<TextAsset>("TA_data_test").text;
        RawDataLevelList levelsFromJSON = null;
        try
        {
            levelsFromJSON = JsonConvert.DeserializeObject<RawDataLevelList>(strJSON, Settings.JsonSettings);
            foreach (var level in levelsFromJSON.RawLevels)
            {
                Level newLevel = new Level();
                newLevel.LevelNumber = level.Level;
                newLevel.TotalTime = level.TotalTime;
                newLevel.TotalScore = level.TotalScore;
                newLevel.TotalCount = level.RawQuestions.Count;
                foreach (var item in level.RawQuestions)
                {
                    Question question = new QuestionText();
                    question.Title = item.Title;
                    question.CountShelves = item.CountShelves;
                    question.QuestionType = (QuestionType)item.QuestionType;
                    question.Score = item.Score;
                    question.Level = level.Level;
                    //Debug.Log(question.QuestionType);
                    question.IsSingleRightAnswer = item.IsSingleRightAnswer;
                    List<Answer> answers = new List<Answer>();
                    foreach (var itemSub in item.RawAnswers)
                    {
                        Answer answer = new Answer();
                        answer.Title = itemSub.Title;
                        answer.IsRight = itemSub.IsRight;
                        answer.Score = itemSub.Score;
                        answer.IsPositionRowDependent = itemSub.IsPositionDependent;
                        answer.IsPositionCellDependent = true;
                        answer.PositionRowIndex = itemSub.PositionRowIndex;
                        answer.PositionCellIndex = itemSub.PositionCellIndex;
                        answers.Add(answer);
                    }
                    question.SetAnswerList(answers);
                    Question.QuestionsList.Add(question);
                }
                Level.Levels.Add(newLevel);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public static void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt("Current_Level", Settings.Current_Level);
    }

    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("Current_Level", 0);
    }

    //public static void SaveCurrentButtonID()
    //{
    //    PlayerPrefs.SetInt("Current_ButtonID", Settings.Current_ButtonID);
    //}

    //public static int GetCurrentButtonID()
    //{
    //    return PlayerPrefs.GetInt("Current_ButtonID", 0);
    //}

    public static void SaveCurrentTheme()
    {
        PlayerPrefs.SetInt("Current_Theme", Settings.Current_Theme);
    }

    public static int GetCurrentTheme()
    {
        return PlayerPrefs.GetInt("Current_Theme", 0);
    }

    public static void SaveLevelResults(int id, int _score, bool _isActive, bool _isPassed, int _activeStarsCount)
    {
        List<ButtonData> buttonDataList = new List<ButtonData>();

        string json = File.ReadAllText(Application.persistentDataPath + Settings.jsonButtonFilePath);


        buttonDataList = JsonConvert.DeserializeObject<List<ButtonData>>(json);


        ButtonData buttonData = buttonDataList.Find(item => item.id == id);

        if (buttonData != null)
        {

            buttonData.score = _score;
            buttonData.isActive = _isActive;
            buttonData.isPassed = _isPassed;
            buttonData.activeStarsCount = _activeStarsCount;

            json = JsonConvert.SerializeObject(buttonDataList, Formatting.Indented);

            File.WriteAllText(Application.persistentDataPath + Settings.jsonButtonFilePath, json);

            //Debug.Log("Button data updated and saved to buttonData.json");
        }
        else
        {
            Debug.LogError("Button data with id " + id + " not found!");
        }

    }

    public static ButtonData GetLevelData(int id)
    {
        List<ButtonData> buttonDataList = new List<ButtonData>();
        string json = "";
        if (File.Exists(Application.persistentDataPath + Settings.jsonButtonFilePath))
            json = File.ReadAllText(Application.persistentDataPath + Settings.jsonButtonFilePath);
        else
        {
            FileStream fs = File.Create(Application.persistentDataPath + Settings.jsonButtonFilePath);
            fs.Dispose();
            TextAsset txt = (TextAsset)Resources.Load("buttonData", typeof(TextAsset));
            json = txt.text;
            //Debug.Log(json);
            File.WriteAllText(Application.persistentDataPath + Settings.jsonButtonFilePath, json);
        }
        //Debug.Log(json);
        buttonDataList = JsonConvert.DeserializeObject<List<ButtonData>>(json);

        ButtonData buttonData = buttonDataList.Find(item => item.id == id);

        if (buttonData != null)
            return buttonData;
        else
        {
            Debug.LogError("Button data with id " + id + " not found!");
            return null;
        }
    }

    public void GetTaskData()
    {
        //List<ButtonData> buttonDataList = new List<ButtonData>();
        string json = "";
        if (File.Exists(Application.persistentDataPath + Settings.jsonTaskFilePath))
        {
            json = File.ReadAllText(Application.persistentDataPath + Settings.jsonTaskFilePath);
            UserData.LoadUserData();
            SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
            StartCoroutine(ComonFunctions.Instance.GetUserTeamID(UserData.UserID));
        }
        else
        {
            StartCoroutine(GetDataFromAPI());
            //FileStream fs = File.Create(Application.persistentDataPath + Settings.jsonTaskFilePath);
            //fs.Dispose();
            //TextAsset txt = (TextAsset)Resources.Load("buttonData", typeof(TextAsset));
            //json = txt.text;
            //File.WriteAllText(Application.persistentDataPath + Settings.jsonButtonFilePath, json);
        }
        //Debug.Log(json);
        //buttonDataList = JsonConvert.DeserializeObject<List<ButtonData>>(json);

        //ButtonData buttonData = buttonDataList.Find(item => item.id == id);

        //if (buttonData != null)
        //    return buttonData;
        //else
        //{
        //    Debug.LogError("Button data with id " + id + " not found!");
        //    return null;
        //}
    }


    private int indexCurrentTheory = 0;
    private int indexCurrentText = 0;
    private int countText = 0; //3;
    private IEnumerator GetDataFromAPI()
    {
        //List<TaskJSONItem> _tasks = new List<TaskJSONItem>();
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("HTTP Error: " + www.error);
            }
            else
            {
                Debug.Log("API Response: " + www.downloadHandler.text);
                TaskJSON response = TaskJSON.FromJson(www.downloadHandler.text);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        ParseJSONTask(item);
                    }
                }

            }
        }
        UserData.LoadUserData();
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
        if(UserData.UserID != "")
            StartCoroutine(ComonFunctions.Instance.GetUserTeamID(UserData.UserID));
    }

    private void ParseJSONTask(TaskJSONItem task)
    {
        try
        {
            string[] raws = task.Content.Split("\r\n");

            Level newLevel;
            if (Level.Levels.Count == 0)
            {
                newLevel = new Level();
                newLevel.LevelNumber = 1;
                newLevel.TotalTime = 300;
                newLevel.TotalScore = task.Points;
                newLevel.TotalCount = raws.Length;
                Level.Levels.Add(newLevel);
            }
            else
            {
                newLevel = Level.Levels[0];
            }

            Question question = new QuestionText();
            question.Title = task.Title;
            question.CountShelves = raws.Length;
            question.QuestionType = QuestionType.Shelf;
            question.Score = task.Points;
            question.Level = task.Topic;
            question.IsSingleRightAnswer = false;
            List<Answer> answers = new List<Answer>();
            int row = 0;
            

            foreach (var strShelf in raws)
            {
                bool isOpenOnStartRow = false;
                string itemShelf = strShelf;
                if (itemShelf.Length > 0 && itemShelf[0] == '?')
                {
                    isOpenOnStartRow = true;
                    itemShelf = itemShelf.Remove(0, 1);
                }

                string[] block = itemShelf.Split(" ");
                int position = 0;
                foreach (var itemSub in block)
                {
                    Answer answer = new Answer();
                    answer.Title = itemSub;
                    answer.IsRight = true;
                    answer.Score = 0;
                    answer.IsPositionCellDependent = true;
                    answer.IsPositionRowDependent = true;
                    answer.PositionRowIndex = row;
                    answer.PositionCellIndex = position;
                    answer.IsOpenOnStart = isOpenOnStartRow;
                    answers.Add(answer);
                    position++;
                    //Debug.Log(itemSub);
                }

                row++;
            }
            question.SetAnswerList(answers);
            Question.QuestionsList.Add(question);

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
