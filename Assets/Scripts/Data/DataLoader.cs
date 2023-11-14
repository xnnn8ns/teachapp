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
using ResponseTestJSON;

public class DataLoader : MonoBehaviour
{
    private const string apiTaskUrl = "http://45.12.239.30:8000/api/tasks/";
    private const string apiTestUrl = "http://45.12.239.30:8000/api/choicefield/";

    float timeStartLoad = 0;

    private void Start()
    {
        SceneManager.LoadScene("WindowLevelLoading", LoadSceneMode.Additive);
        timeStartLoad = Time.time;
        StartCoroutine(GetTestTaskDataFromAPI());
    }


    public static void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt("ButtonOnMapID", Settings.Current_ButtonOnMapID);
    }

    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("ButtonOnMapID", 0);
    }

    public static void SaveCurrentTheme()
    {
        PlayerPrefs.SetInt("Current_Topic", Settings.Current_Topic);
    }

    public static int GetCurrentTheme()
    {
        return PlayerPrefs.GetInt("Current_Topic", 0);
    }

    public static void SaveLevelResults(int id, bool _isActive, bool _isPassed, int _activeStarsCount, int _passCount)
    {
        List<ButtonData> buttonDataList = new List<ButtonData>();

        string json = File.ReadAllText(Application.persistentDataPath + Settings.jsonButtonFilePath);


        buttonDataList = JsonConvert.DeserializeObject<List<ButtonData>>(json);


        ButtonData buttonData = buttonDataList.Find(item => item.id == id);

        if (buttonData != null)
        {

            //buttonData.score = _score;
            buttonData.isActive = _isActive;
            buttonData.isPassed = _isPassed;
            buttonData.activeStarsCount = _activeStarsCount;
            buttonData.passCount = _passCount;

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
            //Debug.LogError("Button data with id " + id + " not found!");
            return null;
        }
    }

    private IEnumerator GetTestTaskDataFromAPI()
    {
        //Debug.Log("GetTestTaskDataFromAPI");
        using (UnityWebRequest www = UnityWebRequest.Get(apiTaskUrl))
        {
            //Debug.Log("SendWebRequest");
            yield return www.SendWebRequest();
            string json = DownLoadOrCreateFileJson(www, Settings.jsonTestFilePath);
            if (json.Length > 0)
            {
                TaskJSON response = TaskJSON.FromJson(json);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                        ParseJSONTask(item);
                }
            }
            else
                Debug.LogError("Error first download test json from server");
        }

        using (UnityWebRequest www = UnityWebRequest.Get(apiTestUrl))
        {
            yield return www.SendWebRequest();
            string json = DownLoadOrCreateFileJson(www, Settings.jsonTaskFilePath);
            if (json.Length > 0)
            {
                TestJSON response = TestJSON.FromJson(json);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                        ParseJSONTest(item);
                }

            }
        }

        UserData.LoadUserData();
        for (int i = 0; i < 10; i++)
        {
            if (timeStartLoad + 3.5f < Time.time)
                break;
            yield return new WaitForSeconds(0.25f);
        }
        
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
        if(UserData.UserID != "")
            StartCoroutine(ComonFunctions.Instance.GetUserTeamID(UserData.UserID));
    }

    private string DownLoadOrCreateFileJson(UnityWebRequest www, string filePathSave)
    {
        string json = "";
        bool isError = false;
        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            //Debug.LogError("HTTP Error: " + www.error);
            isError = true;
            if (File.Exists(Application.persistentDataPath + filePathSave))
            {
                //Debug.Log("File.Exists");
                json = File.ReadAllText(Application.persistentDataPath + filePathSave);
            }
            else
            {
                string newFile = filePathSave.Replace("/","");
                newFile = newFile.Replace(".json", "");
                TextAsset txt = (TextAsset)Resources.Load(newFile, typeof(TextAsset));
                json = txt.text;
            }
        }
        if (!isError)
        {
            json = www.downloadHandler.text;
            if (!File.Exists(Application.persistentDataPath + filePathSave))
            {
                FileStream fs = File.Create(Application.persistentDataPath + filePathSave);
                fs.Dispose();
            }
            File.WriteAllText(Application.persistentDataPath + filePathSave, json);
        }
        return json;
    }

    private void ParseJSONTask(TaskJSONItem task)
    {
        try
        {
            if (task.Content == null)
                task.Content = "";
            string[] raws = task.Content.Split("\r\n");
            if (task.AdditionalBlocks == null)
            {
                task.AdditionalBlocks = "";
                //task.AdditionalBlocks = "8 ! *";
            }
            if (task.AdditionalBlocks.Length == 1 && (task.AdditionalBlocks == "0" || task.AdditionalBlocks == "?"))
                task.AdditionalBlocks = "";

            string[] radditionalBlockRaws = task.AdditionalBlocks.Split("\r\n");

            //Level newLevel = new Level();
            //newLevel.LevelNumber = task.Topic;
            //newLevel.TotalCount = raws.Length;
            //Level.Levels.Add(newLevel);

            Question question = new QuestionText();
            question.Title = task.Title;
            question.Difficulty = task.Difficulty;
            question.CountShelves = raws.Length;
            question.QuestionType = QuestionType.Shelf;
            question.Score = task.Points;
            question.Level = task.Level;
            question.Topic = task.Topic;
            question.Step = task.Step;
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

            if (task.AdditionalBlocks.Length > 0)
            {
                foreach (var strShelf in radditionalBlockRaws)
                {
                    string itemShelf = strShelf;
                    string[] block = itemShelf.Split(" ");
                    //int position = 0;
                    foreach (var itemSub in block)
                    {
                        Answer answer = new Answer();
                        answer.Title = itemSub;
                        answer.IsRight = false;
                        answer.Score = 0;
                        answer.IsPositionCellDependent = false;
                        answer.IsPositionRowDependent = false;
                        answer.PositionRowIndex = 0;
                        answer.PositionCellIndex = 0;
                        answer.IsOpenOnStart = false;
                        answers.Add(answer);
                        //position++;
                        //Debug.Log(itemSub);
                    }

                    //row++;
                }
            }
            question.SetAnswerList(answers);
            Question.QuestionsList.Add(question);

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void ParseJSONTest(TestJSONItem test)
    {
        try
        {
            //Level newLevel = new Level();
            //newLevel.LevelNumber = test.Topic;
            //Level.Levels.Add(newLevel);

            Question question = new QuestionText();
            question.Title = test.Question;
            question.Difficulty = test.Difficulty;
            question.CountShelves = 4;
            question.QuestionType = QuestionType.Test;
            question.Score = test.Points;
            question.Level = test.Level;
            question.Topic = test.Topic;
            question.Step = test.Step;
            //Debug.Log(question.Step);
            question.IsSingleRightAnswer = false;
            List<Answer> answers = new List<Answer>();
            //Debug.Log(question.Score);

            for (int i = 0; i < 4; i++)
            {
                Answer answer = new Answer();
                string answerQuiz = test.Answer1;
                if (i == 1)
                    answerQuiz = test.Answer2;
                else if (i == 2)
                    answerQuiz = test.Answer3;
                else if (i == 3)
                    answerQuiz = test.Answer4;

                answer.Title = answerQuiz;
                answer.IsRight = test.CorrectAnswer == i + 1;
                answer.Score = 0;
                answers.Add(answer);
            }
            question.SetAnswerList(answers);
            //Debug.Log(Question.QuestionsList.Count);
            Question.QuestionsList.Add(question);
            //Debug.Log(Question.QuestionsList.Count);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
