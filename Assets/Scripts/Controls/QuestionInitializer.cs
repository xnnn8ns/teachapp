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

    private Shelf _shelfQuestion;
    private Shelf _shelfAnswer;

    private List<Question> _questions = new List<Question>();
    private List<GameObject> _answers = new List<GameObject>();
    private Question _currentQuestion;

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

    private void InitShelves()
    {
        GameObject shelfQuestionPrefab = Instantiate(_shelfPrefab);
        shelfQuestionPrefab.transform.position = new Vector3(0,4,0);
        shelfQuestionPrefab.transform.localScale = new Vector3(5, 2, 1);
        _shelfQuestion = shelfQuestionPrefab?.GetComponent<Shelf>();

        GameObject shelfAnswerPrefab = Instantiate(_shelfPrefab);
        shelfAnswerPrefab.transform.position = new Vector3(0, -1, 0);
        shelfAnswerPrefab.transform.localScale = new Vector3(5, 2, 1);
        _shelfAnswer = shelfAnswerPrefab?.GetComponent<Shelf>();
    }

    private void InitQuestions()
    {
        FillTestQuestionList();

        int indexQuestion = 0;
        foreach (Question question in _questions)
        {
            GameObject questionPrefab = Instantiate(_questionPrefab);
            QuestionSurface questionSurface = questionPrefab.GetComponent<QuestionSurface>();
            questionSurface.SetTitle(question.Title);
            List<Answer> answers = question.GetAnswerList();
            Vector3 vectorPosition = new Vector3(-_answers.Count - 2f, -1,0);
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
                    _shelfAnswer.AddAnswerToShelf(answerSurface);
            }
            indexQuestion++;
        }
    }

    private void SetAnswerSingleCheck(GameObject answerPrefab, Answer answer, AnimationExecuter animationExecuter)
    {
        AnswerSurface answerSurface = answerPrefab.GetComponent<AnswerSurface>();

        answerSurface.SetTitle(answer.Title);
        answerSurface.SetActionClickCallback(answer,
            animationExecuter,
            AnswerCheckByClick);
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

    //private void InitTouchDetector()
    //{
    //    _touchDetector.StartTouchArise += StartTouch;
    //}

    private void StartTouch(Vector2 vector)
    {
        //var touchPosition = Input.GetTouch(0).position;
        //_firstTouchPosition = _holdTouchPosition = touchPosition;
        //StartTouchArise?.Invoke(touchPosition);
        //LastUserActionTime = Time.timeSinceLevelLoad;
        Debug.Log("touch");
    }

    private void AnswerCheckByClick(Information information, AnimationExecuter transformClicked)
    {
        if(((Answer)information).IsRight)
            transformClicked?.StartUpDownTurn();
        else
            transformClicked?.StartLeftRightTurn();
    }

    private void AnswerTouchDown(AnimationExecuter transformTouchDown, Vector2 position)
    {
        //AnswerSurface answerSurface = transformTouchDown?.GetComponent<AnswerSurface>();
        //if (answerSurface != null)
        //    StartCoroutine(RemoveAnswerFromShelfAfterDelay(answerSurface)); 
        //transformTouchDown?.StartDrag(position);
    }

    public void RemeveFromShelf(Transform transformTouchDown)
    {
        AnswerSurface answerSurface = transformTouchDown?.GetComponent<AnswerSurface>();
        if (answerSurface != null)
            StartCoroutine(RemoveAnswerFromShelfAfterDelay(answerSurface));
    }

    //private void AnswerTouchUp(AnimationExecuter transformTouchUp, Vector2 position)
    //{
    //    transformTouchUp?.StopDrag(position);
    //    AnswerSurface answerSurface = transformTouchUp.GetComponent<AnswerSurface>();
    //    if(answerSurface != null)
    //        CheckAnswerAfterDrop(answerSurface);
    //}

    //private void AnswerTouchMove(AnimationExecuter transformTouchMove, Vector2 position)
    //{
    //    transformTouchMove?.Drag(position);
    //}

    //private void CheckAnswerAfterDrop(AnswerSurface answerSurface)
    //{
    //    if (_shelfQuestion.IsAnswerInsideShelfBorders(answerSurface))
    //        _shelfQuestion.AddAnswerToShelf(answerSurface);
    //    else
    //        _shelfAnswer.AddAnswerToShelf(answerSurface);
    //}

    public void CheckAnswerAfterDrop(Transform transform)
    {
        AnswerSurface answerSurface = transform.GetComponent<AnswerSurface>();
        if (answerSurface == null)
            return;

        if (_shelfQuestion.IsAnswerInsideShelfBorders(answerSurface))
            _shelfQuestion.AddAnswerToShelf(answerSurface);
        else
            _shelfAnswer.AddAnswerToShelf(answerSurface);
    }

    private IEnumerator RemoveAnswerFromShelfAfterDelay(AnswerSurface answerSurface)
    {
        yield return new WaitForSeconds(0.25f);
        _shelfQuestion?.RemoveAnswerFromShelf(answerSurface);
        _shelfAnswer?.RemoveAnswerFromShelf(answerSurface);
        yield break;
    }

    public void ClickCheckAnswerForQuestion()
    {
        List<Answer> answers = new List<Answer>();
        foreach (var item in _shelfQuestion.GetAnswerList())
            answers.Add(item.Answer);
        
        bool isRight = _currentQuestion.IsRightAnswerForQuestion(answers);
        Debug.Log(isRight);
    }
}
