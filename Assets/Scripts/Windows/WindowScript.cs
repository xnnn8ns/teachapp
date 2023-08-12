using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCount;
    [SerializeField] private TextMeshProUGUI _textTime;
    [SerializeField] private TextMeshProUGUI _textScore;

    private string _sceneToLoad = "";

    private void Start()
    {
        _sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        FillWindowData();
    }

    private void FillWindowData()
    {
        //List<Level> levels = QuestionInitializer.GetLevelList();
        foreach (var item in Level.Levels)
        {
            ButtonData buttonData = DataLoader.GetData(Settings.Current_Level);
            int currentLevelWithStarCount = Settings.Current_Level + buttonData.activeStarsCount;
            if (item.LevelNumber == currentLevelWithStarCount)
            {
                _textTime.text = item.TotalTime.ToString();
                _textScore.text = item.TotalScore.ToString();
                _textCount.text = item.TotalCount.ToString();
                break;
            }
        }
    }

    public void ClickCancel()
    {
        SceneManager.UnloadSceneAsync("WindowScene", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    public void ClickOK()
    {
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
    }
}
