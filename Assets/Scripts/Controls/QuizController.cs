using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizController : MonoBehaviour
{
    [SerializeField]
    private QuestionInitializer _questionInitializer;

    private void Awake()
    {
        _questionInitializer.ActionLevelCompleted += ClickReturnFromQuiz;
    }

    public void ClickReturnFromQuiz()
    {
        Scene scene = SceneManager.GetSceneByName("WindowYesNowScene");
        if (scene.isLoaded)
            return;

        SceneManager.LoadScene("WindowYesNowScene", LoadSceneMode.Additive);

        FindObjectOfType<WindowsYesNoMessageScript>()?.FillWindowData(ClickReturnFromQuizOld,"Прекратить задание1");
    }

    public void ClickReturnFromQuizOld()
    {
        Scene scene = SceneManager.GetSceneByName("WindowMessageScene");
        if (scene.isLoaded)
            return;

        //Settings.Current_Level = clickIndex;
        PlayerPrefs.SetString("SceneToLoad", "MapScene");
        SceneManager.LoadScene("WindowMessageScene", LoadSceneMode.Single);

        //SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }
}
