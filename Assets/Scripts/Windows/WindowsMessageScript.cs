using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsMessageScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textScore;
    [SerializeField] private AudioSource _audioScore;

    private string _sceneToLoad = "";
    private int _targetScore = 0;

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        FillWindowData();
    }

    private void FillWindowData()
    {
        _targetScore = PlayerPrefs.GetInt("AddedScore", 0);
        StartCoroutine(ArisePonts(_targetScore));
    }

    public void ClickOK()
    {
        StopAllCoroutines();
        _audioScore?.Stop();
        _textScore.text = _targetScore.ToString();
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
    }

    private IEnumerator ArisePonts(int targetValue)
    {
        int count = 0;
        _audioScore?.Play();
        while (count <= targetValue)
        {
            _textScore.text = count.ToString();
            yield return new WaitForSeconds(0.02f);
            count++;
        }
        _textScore.text = _targetScore.ToString();
        _audioScore?.Stop();
        yield break;
    }
}
