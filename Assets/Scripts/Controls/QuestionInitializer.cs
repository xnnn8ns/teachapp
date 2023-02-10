using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject _questionPrefab;
    [SerializeField]
    private GameObject _answerPrefab;

    private List<Question> _questions = new List<Question>();
    private List<GameObject> _answers = new List<GameObject>();

    private void FillTestQuestionList()
    {
        Question question = new QuestionText();
        question.Title = "How much is the fish?";

        List<Answer> answers = new List<Answer>();

        Answer answer = new Answer();
        answer.Title = "one coin";
        answer.IsRight = false;
        answer.Score = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "two coins";
        answer.IsRight = false;
        answer.Score = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "three coins";
        answer.IsRight = false;
        answer.Score = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "four coins";
        answer.IsRight = true;
        answer.Score = 100;
        answers.Add(answer);

        question.SetAnswerList(answers);

        _questions.Clear();
        _questions.Add(question);
    }

    public void InitQuestions()
    {
        FillTestQuestionList();

        int indexQuestion = 0;
        foreach (Question question in _questions)
        {
            GameObject questionPrefab = Instantiate(_questionPrefab);
            QuestionSurface questionSurface = questionPrefab.GetComponent<QuestionSurface>();
            questionSurface.SetTitle(question.Title);
            List<Answer> answers = question.GetAnswerList();
            Vector3 vectorPosition = new Vector3(-_answers.Count/2, -1,0);
            int indexAnswer = 0;
            foreach (var answer in answers)
            {
                GameObject answerPrefab = Instantiate(_answerPrefab, vectorPosition, Quaternion.identity);
                AnswerSurface answerSurface = answerPrefab.GetComponent<AnswerSurface>();
                answerSurface.SetTitle(answer.Title);
                answerSurface.SetActionCallback(answer, answerPrefab.transform, AnswerCheck);
                vectorPosition += new Vector3(1.5f, 0, 0);
                indexAnswer++;
                _answers.Add(answerPrefab);
            }
            indexQuestion++;
        }
    }

    private void Start()
    {
        InitQuestions();
    }

    private void AnswerCheck(Information information, Transform transformClicked)
    {
        if(((Answer)information).IsRight)
            transformClicked.GetComponent<AnimationExecuter>()?.StartUpDownTurn();
        else
            transformClicked.GetComponent<AnimationExecuter>()?.StartLeftRightTurn();

    }

}
