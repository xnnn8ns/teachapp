using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class DataLoader : MonoBehaviour
{
    private void Awake()
    {
        GetFromJSON();
        UserData.LoadUserData();
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
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
                        answer.IsPositionDependent = itemSub.IsPositionDependent;
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

        string json = File.ReadAllText(Application.dataPath + Settings.jsonFilePath);


        buttonDataList = JsonConvert.DeserializeObject<List<ButtonData>>(json);


        ButtonData buttonData = buttonDataList.Find(item => item.id == id);

        if (buttonData != null)
        {

            buttonData.score = _score;
            buttonData.isActive = _isActive;
            buttonData.isPassed = _isPassed;
            buttonData.activeStarsCount = _activeStarsCount;

            json = JsonConvert.SerializeObject(buttonDataList, Formatting.Indented);

            File.WriteAllText(Application.dataPath + Settings.jsonFilePath, json);

            //Debug.Log("Button data updated and saved to buttonData.json");
        }
        else
        {
            Debug.LogError("Button data with id " + id + " not found!");
        }
    }

    public static ButtonData GetData(int id)
    {
        List<ButtonData> buttonDataList = new List<ButtonData>();
        string json = File.ReadAllText(Application.dataPath + Settings.jsonFilePath);
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
}
