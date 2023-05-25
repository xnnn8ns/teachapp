using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

public class DataLoader : MonoBehaviour
{
    private void Awake()
    {
        GetFromJSON();
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }

    private void GetFromJSON()
    {
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
}
