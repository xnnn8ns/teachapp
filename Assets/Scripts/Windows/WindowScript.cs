using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textHeader;
    [SerializeField]
    private TextMeshProUGUI _textCountHeader;
    [SerializeField]
    private TextMeshProUGUI _textTime;
    [SerializeField]
    private TextMeshProUGUI _textTimeHeader;
    [SerializeField]
    private TextMeshProUGUI _textScore;
    [SerializeField]
    private TextMeshProUGUI _textScoreHeader;
    [SerializeField]
    private TextMeshProUGUI _textButtonOKHeader;
    [SerializeField]
    private bool _isStartTestText = true;
    [SerializeField]
    private AudioSource _clickAudio;

    private string _sceneToLoad = "";
    private string _headerSuffix;
    private string _countHeaderSuffix;

    [Serializable]
    public class TopicNames
    {
        public string topic1;
        public string topic2;
        public string topic3;
    }

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        SetHeaders();
        FillWindowData();
    }

    private void FillWindowData()
    {
        QuestionInitializer.FillQuestionsForCurrentLevel();
        ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
        _textTime.text = ComonFunctions.GetMinetsSecondsFromSeconds(QuestionInitializer.GetSecondsForCurrentQuestionList());
        Debug.Log(buttonData.score.ToString() + "-" + buttonData.passCount);
        if (_textScore != null)
        {
            _textScore.text = ComonFunctions.GetScoreForLevel(buttonData.score, buttonData.passCount, (ETypeLevel)buttonData.typeLevel).ToString();
        }

        // Считать все строки из файла с названиями тем
        TextAsset topicsAsset = Resources.Load<TextAsset>("ListOfTopics");
        TopicNames topicNames = JsonUtility.FromJson<TopicNames>(topicsAsset.text);

        string topicName;
        switch (buttonData.topic)
        {
            case 1:
                topicName = topicNames.topic1;
                break;
            case 2:
                topicName = topicNames.topic2;
                break;
            case 3:
                topicName = topicNames.topic3;
                break;
            default:
                topicName = "Неизвестная тема";
                break;
        }

        if (_textCountHeader != null)
        {
            _textCountHeader.text = "Модуль " + buttonData.topic + ": ";
        }
        if (_textHeader != null)
        {
            _textHeader.text = topicName.ToString();
        }
        int currentProgress = buttonData.score;
        int totalSections = buttonData.passCount;

        // Получить правильное окончание слова "раздел"
        string wordEnding = GetSectionWordEnding(totalSections);

        // Установить текст _textScore и _textScoreHeader
        _textScore.text = currentProgress.ToString();
        _textScoreHeader.text = $"{wordEnding} из <size=100%><b>{totalSections}</b></size>";
    }

    private string GetSectionWordEnding(int number)
    {
        int lastDigit = number % 10;
        int lastTwoDigits = number % 100;

        if (lastDigit == 1 && lastTwoDigits != 11)
        {
            return "раздел";
        }
        else if (lastDigit >= 2 && lastDigit <= 4 && (lastTwoDigits < 10 || lastTwoDigits >= 20))
        {
            return "раздела";
        }
        else
        {
            return "разделов";
        }
    }

    private void SetHeaders()
    {
        if (_textHeader)
        {
            if(_isStartTestText)
                _headerSuffix = LangAsset.GetValueByKey("StartTest");
            else
                _headerSuffix = LangAsset.GetValueByKey("PassingTest");
        }
        if (_textCountHeader)
        {
            _countHeaderSuffix = LangAsset.GetValueByKey("QuizInTest");
        }
        
        _textTimeHeader.text = LangAsset.GetValueByKey("TimeInTest");
        if (_textScoreHeader)
            _textScoreHeader.text = LangAsset.GetValueByKey("PointsInTest");
        _textButtonOKHeader.text = LangAsset.GetValueByKey("Start");
    }

    public void ClickCancel()
    {
        _clickAudio?.Play();
        SceneManager.UnloadSceneAsync("WindowScene", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    public void ClickOK()
    {
        _clickAudio?.Play();
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
    }
}
