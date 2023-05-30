using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsMessageScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textScore;

    private string _sceneToLoad = "";

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        FillWindowData();
    }

    private void FillWindowData()
    {
        _textScore.text = PlayerPrefs.GetInt("AddedScore", 0).ToString();
    }

    public void ClickOK()
    {
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
    }
}
