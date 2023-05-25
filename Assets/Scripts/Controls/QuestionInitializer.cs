using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using Mkey;

public class QuestionInitializer : MonoBehaviour
{
    public event Action ActionLevelCompleted;

    [SerializeField]
    private Text _questionText;
    [SerializeField]
    private GameObject _answerPrefab;
    [SerializeField]
    private GameObject _answerMockPrefab;
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

    
    private List<Question> _questionsCurrentLevel = new List<Question>();
    //private static List<Level> _levels = new List<Level>();
    private List<GameObject> _answers = new List<GameObject>();
    private List<GameObject> _answersMock = new List<GameObject>();
    private static int _currentQuestionIndex = 0;
    private static int _rightAnsweredCount = 0;
    private static int _scoreValue = 0;
    private static float _shelfHeightScale = 0.56f;

    private float heightQuizText = 4.5f;
    private float heightUpperShelf = 2.5f;
    private float heightBelowShelf = -3f;

    

    //private void GetFromJSON()
    //{
    //    string strJSON;
    //    strJSON = Resources.Load<TextAsset>("TA_data_test").text;
    //    RawDataLevelList levelsFromJSON = null;
    //    try
    //    {
    //        levelsFromJSON = JsonConvert.DeserializeObject<RawDataLevelList>(strJSON, Settings.JsonSettings);
    //        foreach (var level in levelsFromJSON.RawLevels)
    //        {
    //            Level newLevel = new Level();
    //            newLevel.LevelNumber = level.Level;
    //            newLevel.TotalTime = level.TotalTime;
    //            newLevel.TotalScore = level.TotalScore;
    //            newLevel.TotalCount = level.RawQuestions.Count;
    //            foreach (var item in level.RawQuestions)
    //            {
    //                Question question = new QuestionText();
    //                question.Title = item.Title;
    //                question.CountShelves = item.CountShelves;
    //                question.QuestionType = (QuestionType)item.QuestionType;
    //                question.Score = item.Score;
    //                question.Level = level.Level;
    //                //Debug.Log(question.QuestionType);
    //                question.IsSingleRightAnswer = item.IsSingleRightAnswer;
    //                List<Answer> answers = new List<Answer>();
    //                foreach (var itemSub in item.RawAnswers)
    //                {
    //                    Answer answer = new Answer();
    //                    answer.Title = itemSub.Title;
    //                    answer.IsRight = itemSub.IsRight;
    //                    answer.Score = itemSub.Score;
    //                    answer.IsPositionDependent = itemSub.IsPositionDependent;
    //                    answer.PositionRowIndex = itemSub.PositionRowIndex;
    //                    answer.PositionCellIndex = itemSub.PositionCellIndex;
    //                    answers.Add(answer);
    //                }
    //                question.SetAnswerList(answers);
    //                _questionsTotal.Add(question);
    //            }
    //            Level.Levels.Add(newLevel);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Log(ex.Message);
    //    }
    //}

    private void FillQuestionsForCurrentLevel()
    {
        _questionsCurrentLevel.Clear();
        _currentQuestionIndex = 0;
        foreach (var question in Question.QuestionsList)
        {
            if (question.Level == Settings.Current_Level)
                _questionsCurrentLevel.Add(question);
        }
    }

    public List<GameObject> GetAnswersList()
    {
        return _answers;
    }

    //public static List<Level> GetLevelList()
    //{
    //    return _levels;
    //}

    private void InitQuestion()
    {
        _imageChecker.gameObject.SetActive(false);
        if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
            InitShelves();
        else if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Test)
            InitTest();
        else
            InitImageTest();
    }

    private void InitShelves()
    {
        int countShelves = _questionsCurrentLevel[_currentQuestionIndex].CountShelves;
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
        _shelfRawAnswers.IsRawAnswersShelf = true;

        InitQuestionTitleAndAnswers();
        Settings.SetDragDropQuestionSettings();
    }

    private void InitTest()
    {
        int countTestShelves = _questionsCurrentLevel[_currentQuestionIndex].GetAnswerCount();
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

        _imageChecker.SetImagesFromAnswers(_questionsCurrentLevel[_currentQuestionIndex].GetAnswerList());

        Settings.SetClickQuestionSettings();
    }

    private void InitQuestionTitleAndAnswers()
    {
        TextAnimation txtAnim = _questionText.GetComponent<TextAnimation>();
        txtAnim.StartType(_questionsCurrentLevel[_currentQuestionIndex].Title, null);
        if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
            InitAnswersForShelf();
        else
            InitAnswersForTest();
    }

    private void InitAnswersForTest()
    {
        List<Answer> answers = _questionsCurrentLevel[_currentQuestionIndex].GetAnswerList();
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
            answerSurface.SetAnswer(answer, true);
            shelf.AddAnswerToShelf(answerSurface);
        }
    }

    private void InitAnswersForShelf()
    {
        List<Answer> answers = _questionsCurrentLevel[_currentQuestionIndex].GetAnswerList();
        Vector3 vectorPosition = new Vector3(-_answers.Count - 2f, -1, 0);

        foreach (var answer in answers)
        { 
            GameObject answerPrefab = Instantiate(_answerPrefab, vectorPosition, Quaternion.identity);
            SetAnswerDrag(answerPrefab, answer, answerPrefab.GetComponent<AnimationExecuter>());
            _answers.Add(answerPrefab);
            AnswerSurface answerSurface = answerPrefab?.GetComponent<AnswerSurface>();
            answerSurface.SetAnswer(answer, true);
            if (answerSurface != null) 
                _shelfRawAnswers.AddAnswerToShelf(answerSurface);

            Vector3 vectorMockPosition = answerPrefab.transform.position + new Vector3(0, 0, 0.01f);
            GameObject mock = Instantiate(_answerMockPrefab, vectorMockPosition, Quaternion.identity);
            mock.transform.localScale = answerSurface.transform.localScale;
            _answersMock.Add(mock);
        }
        _shelfRawAnswers.SetFirstAnswerForRarShelfCompleted();
    }

    private void SetAnswerDrag(GameObject answerPrefab, Answer answer, AnimationExecuter animationExecuter)
    {
        AnswerSurface answerSurface = answerPrefab.GetComponent<AnswerSurface>();
        answerSurface.SetTitle(answer.Title);
    }

    private void Awake()
    {
        //Debug.Log(Settings.Current_Level);
        //FillTestQuestionList();
        _imageChecker.gameObject.SetActive(false);
        //GetFromJSON();
        FillQuestionsForCurrentLevel();
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
        if (_questionsCurrentLevel[_currentQuestionIndex].IsSingleRightAnswer)
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
        if (_currentQuestionIndex >= _questionsCurrentLevel.Count)
        {
            CheckIsLevelCompleted();
            return;
        }

        StopAnimationTextType();

        bool isRight = false;
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
                isRight = _shelvesForCheck[i].IsRightAnswersInShelf(_questionsCurrentLevel[_currentQuestionIndex], i);
            else if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Test)
                isRight = _questionsCurrentLevel[_currentQuestionIndex].GetAnswerList()[i].IsRight == _shelvesForCheck[i].GetTestShelfChecker();
            if (!isRight)
                break;
        }
        if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Image)
            isRight = _imageChecker.GetIsRight();

        Debug.Log(isRight);
        if (isRight)
        {
            _rightAnsweredCount++;
            AddEarnedPoints(_questionsCurrentLevel[_currentQuestionIndex].Score);
        }
        _currentQuestionIndex++;
        DestroyQuestionObjects();
        if (_currentQuestionIndex < _questionsCurrentLevel.Count)
        {
            InitQuestion();
        }
        else
        {
            _imageChecker.gameObject.SetActive(false);
        }
        SetLevelEarnedPoints(_rightAnsweredCount, _questionsCurrentLevel.Count);

        CheckIsLevelCompleted();
    }

    private void DestroyQuestionObjects()
    {
        for (int i = _shelvesForCheck.Count - 1; i >= 0; i--)
        {
            _shelvesForCheck[i].DestroyAllObjectsOnShelf();
            Destroy(_shelvesForCheck[i].gameObject);
        }
        _shelfRawAnswers?.DestroyAllObjectsOnShelf();
        if(_shelfRawAnswers && _shelfRawAnswers.gameObject)
            Destroy(_shelfRawAnswers.gameObject);

        for (int i = _answersMock.Count - 1; i >= 0; i--)
        {
            Destroy(_answersMock[i].gameObject);
        }
        //Destroy(_currentQuestionSurface.gameObject);
        _shelvesForCheck.Clear();
        _answers.Clear();
        _answersMock.Clear();
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
        if (_questionsCurrentLevel[_currentQuestionIndex].IsSingleRightAnswer)
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

    private void StopAnimationTextType()
    {
        _questionText?.GetComponent<TextAnimation>()?.ClickButtonFinishReadingByUser();
    }

    private void CheckIsLevelCompleted()
    {
        if (_currentQuestionIndex >= _questionsCurrentLevel.Count)
            ActionLevelCompleted.Invoke();
    }
}
