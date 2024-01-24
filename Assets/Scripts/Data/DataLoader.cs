using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;
using ResponseTaskJSON;
using ResponseTestJSON;
using ResponseTopicJSON;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class DataLoader : MonoBehaviour, IAppodealInitializationListener
{
    private const string apiTaskUrl = "http://45.12.239.30:8000/api/tasks/";
    private const string apiTestUrl = "http://45.12.239.30:8000/api/choicefield/";
    private const string apiTopicUrl = "http://45.12.239.30:8000/api/topics/";

    float timeStartLoad = 0;

    private void Start()
    {
        int adTypes = Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO | Appodeal.MREC;
        string appKey = "d855dcc5d86d58599d85835161b2e36ba59739faccb88b06";
        if (Application.platform == RuntimePlatform.Android)
        {
            appKey = "d855dcc5d86d58599d85835161b2e36ba59739faccb88b06";
        }

        Appodeal.initialize(appKey, adTypes, this);

        SceneManager.LoadScene("WindowLevelLoading", LoadSceneMode.Additive);
        timeStartLoad = Time.time;
        StartCoroutine(GetTestTaskDataFromAPI());
    }

    public void onInitializationFinished(List<string> errors) { }

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

        using (UnityWebRequest www = UnityWebRequest.Get(apiTopicUrl))
        {
            yield return www.SendWebRequest();
            string json = DownLoadOrCreateFileJson(www, Settings.jsonTopicFilePath);
            if (json.Length > 0)
            {
                TopicJSON response = TopicJSON.FromJson(json);
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                        ParseJSONTopic(item);
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
        
        SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
        if (sceneTransition != null)
        {
            sceneTransition.StartSceneTransition("MapScene");
        }
        else
        {
            SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
        }
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
            Question question = new QuestionText();
            string[] raws = null;

            if (task.Content == null)
                task.Content = "";
            if (task.ContentEn == null)
                task.ContentEn = "";

            if (LangAsset.CurrentLangLocation == LangLocation.Ru)
            {
                question.Title = task.Title;
                raws = task.Content.Split("\r\n");
            }
            else
            {
                question.Title = task.TitleEn;
                if (task.ContentEn == "0")
                    raws = task.Content.Split("\r\n");
                else
                    raws = task.ContentEn.Split("\r\n");
            }

            //string[] raws = task.Content.Split("\r\n");
            if (task.AdditionalBlocks == null)
                task.AdditionalBlocks = "";

            if (task.AdditionalBlocks.Length == 1 && (task.AdditionalBlocks == "0" || task.AdditionalBlocks == "?"))
                task.AdditionalBlocks = "";

            string[] radditionalBlockRaws = task.AdditionalBlocks.Split("\r\n");

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
            Question question = new QuestionText();

            if (LangAsset.CurrentLangLocation == LangLocation.Ru)
                question.Title = test.Question;
            else
                question.Title = test.QuestionEn;
            
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
                string answerQuiz = "";
                string lang = "";
                if (LangAsset.CurrentLangLocation == LangLocation.Ru)
                    lang = "";
                else
                    lang = "En";
                answerQuiz = test.GetType().GetProperty("Answer" + (i+1).ToString() + lang).GetValue(test).ToString();
                if(answerQuiz == "0")
                    answerQuiz = test.GetType().GetProperty("Answer" + (i + 1).ToString()).GetValue(test).ToString();

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

    private static void ParseJSONTopic(TopicJSONItem topic)
    {
        try
        {
            Theory theory = new Theory();

            if (LangAsset.CurrentLangLocation == LangLocation.Ru)
            {
                theory.Title = topic.Title;
                theory.Description = topic.Description;
            }
            else
            {
                theory.Title = topic.TitleEn;
                theory.Description = topic.DescriptionEn;
            }


            theory.ID = topic.Number;
            //Debug.Log(theory.Description);
            Theory.TheoryList.Add(theory);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public static void UpdateLang()
    {
        Theory.TheoryList.Clear();

        string newFile = Settings.jsonTopicFilePath.Replace("/", "");
        newFile = newFile.Replace(".json", "");
        TextAsset txt = (TextAsset)Resources.Load(newFile, typeof(TextAsset));

        string json = txt.text;
        Debug.Log("UpdateLang");
        Debug.Log(json.Length);
        if (json.Length > 0)
        {
            TopicJSON response = TopicJSON.FromJson(json);
            if (response != null && response.Count > 0)
            {
                foreach (var item in response)
                    ParseJSONTopic(item);
            }

        }
    }
}
