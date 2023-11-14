using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textCount;
    [SerializeField]
    private TextMeshProUGUI _textTime;
    [SerializeField]
    private TextMeshProUGUI _textScore;
    [SerializeField]
    private AudioSource _clickAudio;

    private string _sceneToLoad = "";

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        FillWindowData();
    }

    private void FillWindowData()
    {
        QuestionInitializer.FillQuestionsForCurrentLevel();
        ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
        _textTime.text = ComonFunctions.GetMinetsSecondsFromSeconds(QuestionInitializer.GetSecondsForCurrentQuestionList());
        Debug.Log(buttonData.score.ToString() + "-" + buttonData.passCount);
        _textScore.text = ComonFunctions.GetScoreForLevel(buttonData.score, buttonData.passCount, (ETypeLevel)buttonData.typeLevel).ToString();
        _textCount.text = QuestionInitializer.GetQuestionCountCurrentQuestionList().ToString();
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
