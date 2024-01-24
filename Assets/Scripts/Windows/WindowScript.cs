using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;

public class WindowScript : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _textHeader;
    [SerializeField]
    private TextMeshProUGUI _textHeaderValue;
    [SerializeField]
    private TextMeshProUGUI _textCount;
    [SerializeField]
    private TextMeshProUGUI _textCountHeader;
    [SerializeField]
    private TextMeshProUGUI _textTotalCount;
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
        StartCoroutine(CallMethodsAfterOneFrame());
    }

    private IEnumerator CallMethodsAfterOneFrame()
    {
        yield return null;
        FillWindowData();
        SetHeaders();
    }

    private void FillWindowData()
    {
        QuestionInitializer.FillQuestionsForCurrentLevel();
        ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
        _textTime.text = ComonFunctions.GetMinetsSecondsFromSeconds(QuestionInitializer.GetSecondsForCurrentQuestionList());
        Debug.Log(buttonData.score.ToString() + "-" + buttonData.passCount);
        if (_textScore != null)
            _textScore.text = ComonFunctions.GetScoreForLevel(buttonData.score, buttonData.passCount, (ETypeLevel)buttonData.typeLevel).ToString();

        if (_textCount != null){
            int value = buttonData.passCount + 1;
            if(value > buttonData.totalForPassCount)
                value = buttonData.totalForPassCount;
            _textCount.text = value.ToString();
        }

        if(_textTotalCount != null)
            _textTotalCount.text = buttonData.totalForPassCount.ToString();

        
        _slider.value = (buttonData.passCount + 1.2f )/3.0f;
    }

    private void SetHeaders()
    {
        if (_textHeader){
            if(_isStartTestText){
                // string str = LangAsset.GetValueByKey("Section") ;
                // Debug.Log(str);
                // string str1 = Settings.Current_Topic.ToString();
                // Debug.Log(str1);

                // StringBuilder myStringBuilder = new StringBuilder(str);
                // myStringBuilder.Append(str1);

                _textHeader.text = LangAsset.GetValueByKey("Topic" + Settings.Current_Topic.ToString());
                // _textHeader.text += str1;
                // Debug.Log(_textHeader.text);
                // _textHeaderValue.text = LangAsset.GetValueByKey("Topic" + Settings.Current_Topic.ToString());
            }
            else
                _textHeader.text = LangAsset.GetValueByKey("PassingTest");
        }
        if (_textCountHeader){
            _textCountHeader.text = LangAsset.GetValueByKey("Step");

        }
        
        _textTimeHeader.text = LangAsset.GetValueByKey("TimeInTest");
        if (_textScoreHeader)
            _textScoreHeader.text = LangAsset.GetValueByKey("PointsInTest");
        _textButtonOKHeader.text = LangAsset.GetValueByKey("Continue");
        if (_textButtonCancelHeader)
            _textButtonCancelHeader.text = LangAsset.GetValueByKey("Cancel");
    }



    public void ClickCancel()
    {
        //_clickAudio?.Play();
        
        SceneManager.UnloadSceneAsync("WindowScene", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        Vibration.VibratePop();
    }

    public void ClickOK()
    {
        //_clickAudio?.Play();
        
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
        Vibration.VibratePop();
    }
}
