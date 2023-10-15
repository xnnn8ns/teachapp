using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsMessageScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMessage;
    [SerializeField] private TextMeshProUGUI _textValue;
    [SerializeField] private AudioSource _audioScore;
    [SerializeField] private bool _needFillData = true;
    [SerializeField] private bool _needUnloadCurrentScene = false;

    private string _sceneToLoad = "";
    private int _targetScore = 0;

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        if(_needFillData)
            FillWindowData();
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
        StopAllCoroutines();
        _audioScore?.Stop();
        if(_textValue)
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
        while (count <= targetValue)
        {
            _textValue.text = count.ToString();
            yield return new WaitForSeconds(0.02f);
            count++;
        }
        _textValue.text = _targetScore.ToString();
        _audioScore?.Stop();
        yield break;
    }
}
