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
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }
}
