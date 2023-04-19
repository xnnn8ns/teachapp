using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;

public class QuestionInitializer : MonoBehaviour
{
    [SerializeField]
    private Text _questionText;
    [SerializeField]
    private GameObject _answerPrefab;
    [SerializeField]
    private GameObject _shelfPrefab;
    [SerializeField]
    private CanvasController _canvasController;
    [SerializeField]
    private Text _scoreValueText;
    [SerializeField]
    private ImageChecker _imageChecker;

    //private Shelf _shelfQuestion;
    private List<Shelf> _shelvesForCheck = new List<Shelf>();
    private Shelf _shelfRawAnswers;

    private List<Question> _questions = new List<Question>();
    private List<GameObject> _answers = new List<GameObject>();
    private QuestionSurface _currentQuestionSurface;
    private static int _currentQuestionIndex = 0;
    private static int _rightAnsweredCount = 0;
    private static int _scoreValue = 0;
    private static float _shelfHeightScale = 0.56f;

    float heightQuizText = 4.5f;
    float heightUpperShelf = 2.5f;
    float heightBelowShelf = -3f;

    

    private void GetFromJSON()
    {
        string strJSON = "";
        strJSON = Resources.Load<TextAsset>("TA_data").text;
        RawDataQuestion questionFromJSON = null;
        try
        {
            questionFromJSON = JsonConvert.DeserializeObject<RawDataQuestion>(strJSON, Settings.JsonSettings);
            foreach (var item in questionFromJSON.RawQuestions)
            {
                Question question = new QuestionText();
                question.Title = item.Title;
                question.CountShelves = item.CountShelves;
                question.QuestionType = (QuestionType)item.QuestionType;
                question.Score = item.Score;
                //Debug.Log(question.QuestionType);
                question.IsSingleRightAnswer = item.IsSingleRightAnswer;
                List<Answer> answers = new List<Answer>();
                foreach (var itemSub in item.RawAnswers)
                {
                    Answer answer = new Answer();
                    answer.Title = itemSub.Title;
                    answer.IsRight = itemSub.IsRight;
                    answer.Score = itemSub.Score;
                    answer.IsPositionDependent = itemSub.IsPositionDependent;
                    answer.PositionRowIndex = itemSub.PositionRowIndex;
                    answer.PositionCellIndex = itemSub.PositionCellIndex;
                    answers.Add(answer);
                }
                question.SetAnswerList(answers);
                _questions.Add(question);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public List<GameObject> GetAnswersList()
    {
        return _answers;
    }

    private void FillTestQuestionList()
    {
        _questions.Clear();
        _answers.Clear();

        Question question = new QuestionText();
        question.Title = "How much is the fish?";
        question.CountShelves = 2;
        question.IsSingleRightAnswer = false;

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
        answer.PositionRowIndex = 1;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "five coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 1;
        answer.PositionCellIndex = 1;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "six coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 1;
        answer.PositionCellIndex = 2;
        answers.Add(answer);

        question.SetAnswerList(answers);


        _questions.Add(question);

        /////////////////
        ///1


        question = new QuestionText();
        question.Title = "How much is the dog?";
        question.CountShelves = 2;
        question.IsSingleRightAnswer = false;

        answers = new List<Answer>();

        answer = new Answer();
        answer.Title = "one gold coin";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "two gold coins";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 1;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "three gold coins";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 2;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "four gold coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 3;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "five gold coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 1;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "six gold coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 1;
        answer.PositionCellIndex = 1;
        answers.Add(answer);

        question.SetAnswerList(answers);

        _questions.Add(question);

        /////////////////
        ///2


        question = new QuestionText();
        question.Title = "How much is the cat?";
        question.CountShelves = 3;
        question.IsSingleRightAnswer = false;

        answers = new List<Answer>();

        answer = new Answer();
        answer.Title = "one silver coin";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "two silver coins";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 1;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "three silver coins";
        answer.IsRight = true;
        answer.Score = 0;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 1;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "four silver coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 1;
        answer.PositionCellIndex = 1;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "five silver coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 2;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "six silver coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = true;
        answer.PositionRowIndex = 2;
        answer.PositionCellIndex = 1;
        answers.Add(answer);

        question.SetAnswerList(answers);

        _questions.Add(question);


        /////////////
        ///3
        ///

        question = new QuestionText();
        question.Title = "How much is the bear?";
        question.CountShelves = 1;
        question.IsSingleRightAnswer = true;

        answers = new List<Answer>();

        answer = new Answer();
        answer.Title = "one brilliant coin";
        answer.IsRight = false;
        answer.Score = 0;
        answer.IsPositionDependent = false;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "two brilliant coins";
        answer.IsRight = false;
        answer.Score = 0;
        answer.IsPositionDependent = false;
        answer.PositionRowIndex = 0;
        answer.PositionCellIndex = 0;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "three brilliant coins";
        answer.IsRight = false;
        answer.Score = 0;
        answer.IsPositionDependent = false;
        answers.Add(answer);

        answer = new Answer();
        answer.Title = "four brilliant coins";
        answer.IsRight = true;
        answer.Score = 100;
        answer.IsPositionDependent = false;
        answers.Add(answer);

        question.SetAnswerList(answers);

        _questions.Add(question);
    }

    private void InitQuestion()
    {
        _imageChecker.gameObject.SetActive(false);
        if (_questions[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
            InitShelves();
        else if (_questions[_currentQuestionIndex].QuestionType == QuestionType.Test)
            InitTest();
        else
            InitImageTest();
    }

    private void InitShelves()
    {
        int countShelves = _questions[_currentQuestionIndex].CountShelves;
        _answers.Clear();
        _shelvesForCheck.Clear();

        for (int i = 0; i < countShelves; i++)
        {
            GameObject shelfQuestionPrefab = Instantiate(_shelfPrefab);
            shelfQuestionPrefab.transform.position = new Vector3(0, heightUpperShelf - i * _shelfHeightScale * 1.05f, 0);
            shelfQuestionPrefab.transform.localScale = new Vector3(5, _shelfHeightScale, 1);
            _shelvesForCheck.Add(shelfQuestionPrefab?.GetComponent<Shelf>());
        }

        GameObject shelfAnswerPrefab = Instantiate(_shelfPrefab);
        shelfAnswerPrefab.transform.position = new Vector3(0, heightBelowShelf, 0);
        shelfAnswerPrefab.transform.localScale = new Vector3(5, 3, 1);
        _shelfRawAnswers = shelfAnswerPrefab?.GetComponent<Shelf>();

        InitQuestionTitleAndAnswers();
        Settings.SetDragDropQuestionSettings();
    }

    private void InitTest()
    {
        int countTestShelves = _questions[_currentQuestionIndex].GetAnswerCount();
        _answers.Clear();
        _shelvesForCheck.Clear();

        for (int i = 0; i < countTestShelves; i++)
        {
            GameObject shelfAnswerPrefab = Instantiate(_shelfPrefab);
            shelfAnswerPrefab.transform.position = new Vector3(0, heightUpperShelf - i * _shelfHeightScale * 1.05f, 0);
            shelfAnswerPrefab.transform.localScale = new Vector3(5, _shelfHeightScale, 1);
            Shelf shelf = shelfAnswerPrefab?.GetComponent<Shelf>();
            if (shelf != null)
            {
                _shelvesForCheck.Add(shelf);
                shelf.ClickShelf += ClickTestShelf;
            }
        }

        InitQuestionTitleAndAnswers();
        Settings.SetClickQuestionSettings();
    }

    private void InitImageTest()
    {
        _imageChecker.gameObject.SetActive(true);
        _answers.Clear();
        _shelvesForCheck.Clear();

        _imageChecker.SetImagesFromAnswers(_questions[_currentQuestionIndex].GetAnswerList());

        Settings.SetClickQuestionSettings();
    }

    private void InitQuestionTitleAndAnswers()
    {
        TextAnimation txtAnim = _questionText.GetComponent<TextAnimation>();
        txtAnim.StartType(_questions[_currentQuestionIndex].Title, null);
        if (_questions[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
            InitAnswersForShelf();
        else
            InitAnswersForTest();
    }

    private void InitAnswersForTest()
    {
        List<Answer> answers = _questions[_currentQuestionIndex].GetAnswerList();
        //Vector3 vectorPosition = new Vector3(-_answers.Count - 2f, -1, 0);

        if (answers.Count != _shelvesForCheck.Count)
            return;

        for (int i = 0; i < answers.Count; i++)
        {
            Answer answer = answers[i];
            Shelf shelf = _shelvesForCheck[i];

            Vector3 vectorPosition = shelf.gameObject.transform.position;

            GameObject answerPrefab = Instantiate(_answerPrefab, vectorPosition, Quaternion.identity);

            SetAnswerDrag(answerPrefab, answer, answerPrefab.GetComponent<AnimationExecuter>());

            _answers.Add(answerPrefab);
            AnswerSurface answerSurface = answerPrefab?.GetComponent<AnswerSurface>();
            answerSurface.Answer = answer;
            shelf.AddAnswerToShelf(answerSurface);
        }
    }

    private void InitAnswersForShelf()
    {
        List<Answer> answers = _questions[_currentQuestionIndex].GetAnswerList();
        Vector3 vectorPosition = new Vector3(-_answers.Count - 2f, -1, 0);

        foreach (var answer in answers)
        {
            GameObject answerPrefab = Instantiate(_answerPrefab, vectorPosition, Quaternion.identity);

            SetAnswerDrag(answerPrefab, answer, answerPrefab.GetComponent<AnimationExecuter>());
            //float yMult = countRows * 1.15f;
            //vectorPosition += new Vector3(1.15f, 0, 0);
            _answers.Add(answerPrefab);
            AnswerSurface answerSurface = answerPrefab?.GetComponent<AnswerSurface>();
            answerSurface.Answer = answer;
            if (answerSurface != null) 
                _shelfRawAnswers.AddAnswerToShelf(answerSurface);
            //if(vectorPosition.x + 1f > 3f)
            //    countRows++;
            //Debug.Log(countRows);
        }
    }

    private void SetAnswerDrag(GameObject answerPrefab, Answer answer, AnimationExecuter animationExecuter)
    {
        AnswerSurface answerSurface = answerPrefab.GetComponent<AnswerSurface>();
        answerSurface.SetTitle(answer.Title);
    }

    private void Start()
    {
        //FillTestQuestionList();
        _imageChecker.gameObject.SetActive(false);
        GetFromJSON();
        InitQuestion();
        //InitTouchDetector();
    }

    public void RemoveFromShelf(Transform transformTouchDown)
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
        if (_questions[_currentQuestionIndex].IsSingleRightAnswer)
            CheckSingleAnswerAfterDrop(answerSurface);
        else
            CheckMultiAnswerAfterDrop(answerSurface);
    }

    private void CheckMultiAnswerAfterDrop(AnswerSurface answerSurface)
    {
        bool isInsideAnyShelf = false;
        foreach (Shelf shelf in _shelvesForCheck)
        {
            if (shelf.IsAnswerInsideShelfBorders(answerSurface))
            {
                shelf.AddAnswerToShelfByDrag(answerSurface);
                isInsideAnyShelf = true;
                break;
            }
        }
        
        if(!isInsideAnyShelf)
            _shelfRawAnswers.AddAnswerToShelfByDrag(answerSurface);
    }

    private void CheckSingleAnswerAfterDrop(AnswerSurface answerSurface)
    {
        
        bool isInsideAnyShelf = false;
        foreach (Shelf shelf in _shelvesForCheck)
        {
            if (shelf.IsAnswerInsideShelfBorders(answerSurface)
                && !shelf.Equals(_shelfRawAnswers))
            {
                MoveAllAnswersToOtherShelf(shelf, _shelfRawAnswers);
                shelf.AddAnswerToShelfByDrag(answerSurface);
                isInsideAnyShelf = true;
                break;
            }
        }

        if (!isInsideAnyShelf)
            _shelfRawAnswers.AddAnswerToShelfByDrag(answerSurface);
    }

    private void MoveAllAnswersToOtherShelf(Shelf source, Shelf destination)
    {
        foreach (AnswerSurface answerSurface in source.GetAnswerList())
        {
            destination.AddAnswerToShelf(answerSurface);
        }
        source.GetAnswerList().Clear();
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
        if (_currentQuestionIndex >= _questions.Count)
            return;

        bool isRight = false;
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (_questions[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
                isRight = _shelvesForCheck[i].IsRightAnswersInShelf(_questions[_currentQuestionIndex], i);
            else if (_questions[_currentQuestionIndex].QuestionType == QuestionType.Test)
                isRight = _questions[_currentQuestionIndex].GetAnswerList()[i].IsRight == _shelvesForCheck[i].GetTestShelfChecker();
            if (!isRight)
                break;
        }
        if (_questions[_currentQuestionIndex].QuestionType == QuestionType.Image)
            isRight = _imageChecker.GetIsRight();

        Debug.Log(isRight);
        if (isRight)
        {
            _rightAnsweredCount++;
            AddEarnedPoints(_questions[_currentQuestionIndex].Score);
        }
        _currentQuestionIndex++;
        DestroyQuestionObjects();
        if (_currentQuestionIndex < _questions.Count)
        {
            InitQuestion();
        }
        else
        {
            _imageChecker.gameObject.SetActive(false);
        }
        SetLevelEarnedPoints(_rightAnsweredCount, _questions.Count);
    }

    private void DestroyQuestionObjects()
    {
        for (int i = _shelvesForCheck.Count - 1; i >= 0; i--)
        {
            _shelvesForCheck[i].DestroyAllObjectsOnShelf();
            Destroy(_shelvesForCheck[i].gameObject);
        }
        _shelfRawAnswers.DestroyAllObjectsOnShelf();
        if(_shelfRawAnswers && _shelfRawAnswers.gameObject)
            Destroy(_shelfRawAnswers.gameObject);
        //Destroy(_currentQuestionSurface.gameObject);
        _shelvesForCheck.Clear();
        _answers.Clear();
    }

    private void SetLevelEarnedPoints(int rightAnsweredQuestion, int totalQuestionCount)
    {
        if (totalQuestionCount == 0)
            return;

        _canvasController.SetLevelProgress((float)rightAnsweredQuestion / totalQuestionCount);
    }

    private void AddEarnedPoints(int valuePoints)
    {
        _scoreValue += valuePoints;
        _scoreValueText.text = _scoreValue.ToString();
    }

    private void ClickTestShelf(bool value, Shelf shelf)
    {
        //Debug.Log("Click: " + value);
        if (_questions[_currentQuestionIndex].IsSingleRightAnswer)
        {
            foreach (Shelf shl in _shelvesForCheck)
            {
                if (shl.Equals(shelf))
                {
                    if (!shelf.GetTestShelfChecker())
                        shl.SetTestShelfChecker(true);
                }
                else
                    shl.SetTestShelfChecker(false);
            }
        }
    }
}
