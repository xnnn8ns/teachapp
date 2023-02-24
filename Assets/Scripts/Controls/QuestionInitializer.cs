using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject _questionPrefab;
    [SerializeField]
    private GameObject _answerPrefab;
    [SerializeField]
    private GameObject _shelfPrefab;

    //private Shelf _shelfQuestion;
    private List<Shelf> _shelvesForCheck = new List<Shelf>();
    private Shelf _shelfRawAnswers;

    private List<Question> _questions = new List<Question>();
    private List<GameObject> _answers = new List<GameObject>();
    private Question _currentQuestion;

    float heightQuizText = 4.5f;
    float heightUpperShelf = 2.5f;
    float heightBelowShelf = -3.5f;


    public List<GameObject> GetAnswersList()
    {
        return _answers;
    }

    private void FillTestQuestionList()
    {
        Question question = new QuestionText();
        question.Title = "How much is the fish?";

        List<Answer> answers = new List<Answer>();

        Answer answer = new Answer();
        answer.Title = "one coin";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "two coins";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 1;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "three coins";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 2;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "four coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 3;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "five coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 1;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "six coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 2;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        question.SetAnswerList(answers);

        _questions.Clear();
        _questions.Add(question);
    }

    private void InitShelves()
    {
        int countShelves = 4;

        for (int i = 0; i < countShelves; i++)
        {
            GameObject shelfQuestionPrefab = Instantiate(_shelfPrefab);
            shelfQuestionPrefab.transform.position = new Vector3(0, heightUpperShelf - i * 1.05f, 0);
            shelfQuestionPrefab.transform.localScale = new Vector3(5, 1, 1);
            _shelvesForCheck.Add(shelfQuestionPrefab?.GetComponent<Shelf>());
        }

        GameObject shelfAnswerPrefab = Instantiate(_shelfPrefab);
        shelfAnswerPrefab.transform.position = new Vector3(0, heightBelowShelf, 0);
        shelfAnswerPrefab.transform.localScale = new Vector3(5, 2, 1);
        _shelfRawAnswers = shelfAnswerPrefab?.GetComponent<Shelf>();
    }

    private void InitQuestions()
    {
        FillTestQuestionList();

        int indexQuestion = 0;
        foreach (Question question in _questions)
        {
            GameObject questionPrefab = Instantiate(_questionPrefab);
            questionPrefab.transform.position = new Vector3(0, heightQuizText, 0);

            QuestionSurface questionSurface = questionPrefab.GetComponent<QuestionSurface>();
            questionSurface.SetTitle(question.Title);
            List<Answer> answers = question.GetAnswerList();
            Vector3 vectorPosition = new Vector3(-_answers.Count - 2f, -1, 0);
            int indexAnswer = 0;
            _currentQuestion = question;
            foreach (var answer in answers)
            {
                GameObject answerPrefab = Instantiate(_answerPrefab, vectorPosition, Quaternion.identity);

                SetAnswerDrag(answerPrefab, answer, answerPrefab.GetComponent<AnimationExecuter>());

                vectorPosition += new Vector3(1.15f, 0, 0);
                indexAnswer++;
                _answers.Add(answerPrefab);
                AnswerSurface answerSurface = answerPrefab?.GetComponent<AnswerSurface>();
                answerSurface.Answer = answer;
                if (answerSurface != null)
                    _shelfRawAnswers.AddAnswerToShelf(answerSurface);
            }
            indexQuestion++;
        }
    }

    private void SetAnswerDrag(GameObject answerPrefab, Answer answer, AnimationExecuter animationExecuter)
    {
        AnswerSurface answerSurface = answerPrefab.GetComponent<AnswerSurface>();

        answerSurface.SetTitle(answer.Title);
        //answerSurface.SetActionDownCallback(answer,
        //    animationExecuter,
        //    AnswerTouchDown);
        //answerSurface.SetActionUpCallback(answer,
        //    animationExecuter,
        //    AnswerTouchUp);
        //answerSurface.SetActionDragCallback(answer,
        //    animationExecuter,
        //    AnswerTouchMove);
    }

    private void Start()
    {
        InitShelves();
        InitQuestions();
        //InitTouchDetector();
    }

    public void RemeveFromShelf(Transform transformTouchDown)
    {
        AnswerSurface answerSurface = transformTouchDown?.GetComponent<AnswerSurface>();
        if (answerSurface != null)
            StartCoroutine(RemoveAnswerFromShelfAfterDelay(answerSurface));
    }

    public void CheckAnswerAfterDrop(Transform transform)
    {
        AnswerSurface answerSurface = transform.GetComponent<AnswerSurface>();
        if (answerSurface == null)
            return;
        bool isInsideAnyShelf = false;
        foreach (Shelf shelf in _shelvesForCheck)
        {
            if (shelf.IsAnswerInsideShelfBorders(answerSurface))
            {
                shelf.AddAnswerToShelf(answerSurface);
                isInsideAnyShelf = true;
                break;
            }
        }
        
        if(!isInsideAnyShelf)
            _shelfRawAnswers.AddAnswerToShelf(answerSurface);
    }

    private IEnumerator RemoveAnswerFromShelfAfterDelay(AnswerSurface answerSurface)
    {
        yield return new WaitForSeconds(0.25f);
        foreach (Shelf shelf in _shelvesForCheck)
        {
            if (shelf.GetAnswerList().Contains(answerSurface))
            {
                shelf.RemoveAnswerFromShelf(answerSurface);
                break;
            }
        }
        _shelfRawAnswers?.RemoveAnswerFromShelf(answerSurface);
        yield break;
    }

    public void ClickCheckAnswerForQuestion()
    {
        bool isRight = false;
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            isRight = _shelvesForCheck[i].IsRightAnswersInShelf(_currentQuestion,i);
            if (!isRight)
                break;
        }
        Debug.Log(isRight);
    }
}
