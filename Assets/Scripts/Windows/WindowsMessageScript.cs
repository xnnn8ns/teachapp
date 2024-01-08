using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsMessageScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textHeader;
    [SerializeField]
    private TextMeshProUGUI _textMessage;
    [SerializeField]
    private TextMeshProUGUI _textValue;
    [SerializeField]
    private TextMeshProUGUI _textValueHeader;
    [SerializeField]
    private TextMeshProUGUI _textButtonOKHeader;
    [SerializeField]
    private AudioSource _audioScore;
    [SerializeField]
    private AudioSource _clickAudio;
    [SerializeField]
    private bool _isErrorExistsForm = false;
    [SerializeField]
    private bool _isLevelBlockedForm = false;
    [SerializeField]
    private bool _isTimeExpiredForm = false;

    [SerializeField]
    private AudioSource _audioFanFars;
    [SerializeField]
    private bool _needFillData = true;
    [SerializeField]
    private bool _needUnloadCurrentScene = false;
    [SerializeField]
    

    private string _sceneToLoad = "";
    private int _targetScore = 0;

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        if(_needFillData)
            FillWindowData();
        SetHeaders();
    }

    private void FillWindowData()
    {
        if (_textValue)
        {
            _targetScore = PlayerPrefs.GetInt("AddedScore", 0);
            StartCoroutine(ArisePonts(_targetScore));
        }
        if (_textMessage)
        {
            _textMessage.text = PlayerPrefs.GetString("MessageForWindow", "");
        }
    }

    public void ClickOK()
    {
        //_clickAudio?.Play();
        Vibration.VibratePop();
        StopAllCoroutines();
        if(_audioScore)
            _audioScore?.Stop();
        if (_audioFanFars)
            _audioFanFars?.Stop();
        if (_textValue)
            _textValue.text = _targetScore.ToString();
        if (_needUnloadCurrentScene)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == "WindowRepeatErrorScene")
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                if (SceneManager.GetSceneAt(i).name == "WindowSimpliMessageScene")
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
        else
            SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);

        Settings.IsModalWindowOpened = false;
    }

    private IEnumerator ArisePonts(int targetValue)
    {
        int count = 0;
        _audioScore?.Play();
        int deltaStep = 1;
        if (targetValue < 25)
            deltaStep = 1;
        else if (targetValue < 50)
            deltaStep = 2;
        else if (targetValue < 80)
            deltaStep = 3;
        else if (targetValue < 150)
            deltaStep = 4;
        else if (targetValue < 250)
            deltaStep = 5;
        else if (targetValue < 500)
            deltaStep = 10;
        else if (targetValue < 1000)
            deltaStep = 20;
        else
            deltaStep = 100;
        while (count < targetValue)
        {
            _textValue.text = count.ToString();
            yield return new WaitForSeconds(0.02f);
            count += deltaStep;
            if (count > targetValue)
                count = targetValue;
        }
        _textValue.text = _targetScore.ToString();
        _audioScore?.Stop();
        
        _audioFanFars?.Play();
        yield return new WaitForSeconds(2f);
        _audioFanFars?.GetComponent<ParticleSystem>()?.Stop();
        yield return new WaitForSeconds(1f);
        _audioFanFars?.Stop();
        yield break;
    }

    private void SetHeaders()
    {
        if (_textHeader)
        {
            if (_isErrorExistsForm)
                _textHeader.text = LangAsset.GetValueByKey("HaveIncorrectAnswers");
            else if (_isTimeExpiredForm)
                _textHeader.text = LangAsset.GetValueByKey("TimeIsUp");
            else
                _textHeader.text = LangAsset.GetValueByKey("TestPassed");
        }
        if(!_isErrorExistsForm && _textValueHeader)
            _textValueHeader.text = LangAsset.GetValueByKey("PointsReceived");
        if (_isErrorExistsForm)
            _textMessage.text = LangAsset.GetValueByKey("TryAgain");
        else if (_isLevelBlockedForm)
        {
            string x = LangAsset.GetValueByKey("LevelBlocked").Replace("|||", System.Environment.NewLine);
            _textMessage.text = x;
        }else if(_isTimeExpiredForm)
            _textMessage.text = LangAsset.GetValueByKey("TaskFailed");

        if (_isLevelBlockedForm)
            _textButtonOKHeader.text = LangAsset.GetValueByKey("Close");
        else
            _textButtonOKHeader.text = LangAsset.GetValueByKey("Continue");

    }
}
