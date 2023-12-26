using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textHeader;
    [SerializeField]
    private TextMeshProUGUI _textCount;
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
    private TextMeshProUGUI _textButtonCancelHeader;
    [SerializeField]
    private bool _isStartTestText = true;
    [SerializeField]
    private AudioSource _clickAudio;

    private string _sceneToLoad = "";

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        FillWindowData();
        SetHeaders();
    }

    private void FillWindowData()
    {
        QuestionInitializer.FillQuestionsForCurrentLevel();
        ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
        _textTime.text = ComonFunctions.GetMinetsSecondsFromSeconds(QuestionInitializer.GetSecondsForCurrentQuestionList());
        Debug.Log(buttonData.score.ToString() + "-" + buttonData.passCount);
        if (_textScore != null){
            _textScore.text = ComonFunctions.GetScoreForLevel(buttonData.score, buttonData.passCount, (ETypeLevel)buttonData.typeLevel).ToString();
            _textCount.text = QuestionInitializer.GetQuestionCountCurrentQuestionList().ToString();
        }
    }

    private void SetHeaders()
    {
        if (_textHeader){
            if(_isStartTestText)
                _textHeader.text = LangAsset.GetValueByKey("StartTest");
            else
                _textHeader.text = LangAsset.GetValueByKey("PassingTest");
        }
        if (_textCountHeader)
            _textCountHeader.text = LangAsset.GetValueByKey("QuizInTest");
        
        _textTimeHeader.text = LangAsset.GetValueByKey("TimeInTest");
        if (_textScoreHeader)
            _textScoreHeader.text = LangAsset.GetValueByKey("PointsInTest");
        _textButtonOKHeader.text = LangAsset.GetValueByKey("Start");
        if (_textButtonCancelHeader)
            _textButtonCancelHeader.text = LangAsset.GetValueByKey("Cancel");
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
