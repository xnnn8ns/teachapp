using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizController : MonoBehaviour
{
    public void ClickReturnFromQuiz()
    {
        SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
    }
}
