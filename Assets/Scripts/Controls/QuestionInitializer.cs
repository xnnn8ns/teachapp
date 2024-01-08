using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using Mkey;
using UnityEngine.SceneManagement;

public class QuestionInitializer : MonoBehaviour
{
    #region vars

    public event Action ActionLevelCompleted;

    [SerializeField]
    private Text _questionText;
    [SerializeField]
    private GameObject _answerPrefab;
    [SerializeField]
    private GameObject _answerReadyPrefab;
    [SerializeField]
    private GameObject _answerMockPrefab;
    [SerializeField]
    private GameObject _shelfPrefab;
    [SerializeField]
    private CanvasController _canvasController;
    [SerializeField]
    private Text _scoreValueText;
    [SerializeField]
    private ImageChecker _imageCheckerTest;
    [SerializeField]
    private ImageChecker _imageCheckerShelfTest;
    [SerializeField]
    private GameObject _resultPanelScript;
    [SerializeField]
    private GameObject _buttonCheck;
    [SerializeField]
    private GameObject _buttonCheckDisabled;

    private AudioSource _clickAudio;
    private AudioSource _typeAudio;
    private AudioSource _wrongAnswerAudio;
    private AudioSource _OKAnswerAudio;
    private AudioSource _clickCatImageAudio;

    private List<Shelf> _shelvesForCheck = new List<Shelf>();
    private Shelf _shelfRawAnswers;
    private Shelf _shelfDefault;


    private static List<Question> _questionsCurrentLevel = new List<Question>();
    private List<GameObject> _answers = new List<GameObject>();
    private static int _currentQuestionIndex = 0;
    private static int _rightAnsweredCount = 0;
    //private static int _scoreValue = 0;
    private static float _shelfHeightScale = 0.56f;

    private float _heightUpperShelf = 2.5f;
    private float _heightBelowShelf = -3f;

    private int _secondsCountToLeft = 100;
    private int _secondsCountOnStart = 100;
    #endregion

    public static void FillQuestionsForCurrentLevel()
    {
        //_scoreValue = 0;
        _questionsCurrentLevel.Clear();
        _currentQuestionIndex = 0;
        //Debug.Log(Settings.Current_ButtonOnMapID);
        ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
        //Debug.Log(buttonData.id);
        
        if (buttonData != null)
        {
            _questionsCurrentLevel = Question.GetQuestionListForLevel(
                buttonData.topic,
                Settings.Current_ButtonOnMapID,
                buttonData.passCount,
                (ETypeLevel)buttonData.typeLevel);
        }
    }

    public List<GameObject> GetAnswersList()
    {
        return _answers;
    }

    private void Awake()
    {
        _clickAudio = GetComponents<AudioSource>()[0];
        _typeAudio = GetComponents<AudioSource>()[1];
        _wrongAnswerAudio = GetComponents<AudioSource>()[2];
        _OKAnswerAudio = GetComponents<AudioSource>()[3];
        _clickCatImageAudio = GetComponents<AudioSource>()[4];
        //FillTestQuestionList();
        _imageCheckerTest.gameObject.SetActive(false);
        //GetFromJSON();
        //FillQuestionsForCurrentLevel();
        InitQuestion();
        //InitTouchDetector();
        _resultPanelScript.SetActive(false);
    }

    #region Init

    private void InitQuestion()
    {
        _resultPanelScript.SetActive(false);
        Debug.Log(_resultPanelScript.activeSelf);
        _imageCheckerTest.gameObject.SetActive(false);
        _imageCheckerShelfTest.gameObject.SetActive(false);
        if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
            InitShelves();
        else if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Test)
            InitTestSimple();
        else if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.ShelfTest)
            InitShelveTests();
        //else
        //    InitImageCheck();
        if (_currentQuestionIndex == 0) {
            _secondsCountToLeft = GetSecondsForCurrentQuestionList();
            _secondsCountOnStart = _secondsCountToLeft;
        }

        StartCoroutine(TimerCountDown(_secondsCountToLeft));

        _buttonCheck.SetActive(false);
        _buttonCheckDisabled.SetActive(true);
        
    }

    private void InitShelves()
    {
        int countShelves = _questionsCurrentLevel[_currentQuestionIndex].CountShelves;
        _answers.Clear();
        _shelvesForCheck.Clear();

        for (int i = 0; i < countShelves; i++)
        {
            GameObject shelfQuestionPrefab = Instantiate(_shelfPrefab);
            shelfQuestionPrefab.transform.position = new Vector3(0, _heightUpperShelf - i * _shelfHeightScale * 1.05f, -0.01f);

            shelfQuestionPrefab.transform.localScale = new Vector3(ComonFunctions.GetScaleForShelf(), _shelfHeightScale, 1);
            _shelvesForCheck.Add(shelfQuestionPrefab?.GetComponent<Shelf>());
        }

        GameObject shelfAnswerPrefab = Instantiate(_shelfPrefab);
        shelfAnswerPrefab.transform.position = new Vector3(0, _heightBelowShelf, -0.01f);
        shelfAnswerPrefab.transform.localScale = new Vector3(ComonFunctions.GetScaleForShelf(), 3, 1);
        _shelfRawAnswers = shelfAnswerPrefab?.GetComponent<Shelf>();
        _shelfRawAnswers.SetAsRawShelf();
        //_materialFull

        InitQuestionTitleAndAnswers();
        Settings.SetDragDropQuestionSettings();
    }

    private void InitShelveTests()
    {
        int countShelves = _questionsCurrentLevel[_currentQuestionIndex].CountShelves;
        _answers.Clear();
        _shelvesForCheck.Clear();
        _imageCheckerShelfTest.gameObject.SetActive(true);
        for (int i = 0; i < countShelves; i++)
        {
            GameObject shelfQuestionPrefab = Instantiate(_shelfPrefab);
            shelfQuestionPrefab.transform.position = new Vector3(0, _heightUpperShelf - i * _shelfHeightScale * 1.05f, -0.01f);
            
            shelfQuestionPrefab.transform.localScale = new Vector3(ComonFunctions.GetScaleForShelf(), _shelfHeightScale, 1);
            _shelvesForCheck.Add(shelfQuestionPrefab?.GetComponent<Shelf>());
        }

        GameObject shelfAnswerPrefab = Instantiate(_shelfPrefab);
        shelfAnswerPrefab.transform.position = new Vector3(0, _heightBelowShelf, -0.01f);
        shelfAnswerPrefab.transform.localScale = new Vector3(ComonFunctions.GetScaleForShelf(), 3, 1);
        _shelfRawAnswers = shelfAnswerPrefab?.GetComponent<Shelf>();
        _shelfRawAnswers.SetAsRawShelf(false);

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
            shelfAnswerPrefab.transform.position = new Vector3(0, _heightUpperShelf - i * _shelfHeightScale * 1.05f, -0.01f);
            shelfAnswerPrefab.transform.localScale = new Vector3(ComonFunctions.GetScaleForShelf(), _shelfHeightScale, 1);
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

    private void InitImageCheck()
    {
        _imageCheckerTest.gameObject.SetActive(true);
        _answers.Clear();
        _shelvesForCheck.Clear();

        _imageCheckerTest.SetImagesFromAnswers(_questionsCurrentLevel[_currentQuestionIndex].GetAnswerList(), ClickImageTest);

        Settings.SetClickQuestionSettings();
    }

    private void InitTestSimple()
    {
        _imageCheckerTest.gameObject.SetActive(true);
        _answers.Clear();
        _shelvesForCheck.Clear();

        _imageCheckerTest.SetTestFromAnswers(_questionsCurrentLevel[_currentQuestionIndex].GetAnswerList(), ClickImageTest);
        InitQuestionTitleAndAnswers();
        Settings.SetClickQuestionSettings();
    }

    private void InitQuestionTitleAndAnswers()
    {
        TextAnimation txtAnim = _questionText.GetComponent<TextAnimation>();
        txtAnim.StartType(_questionsCurrentLevel[_currentQuestionIndex].Title, FinishTypeTextCallBack);
        switch (_questionsCurrentLevel[_currentQuestionIndex].QuestionType)
        {
            case QuestionType.Shelf:
                InitAnswersForShelf();
                break;
            case QuestionType.Test:
                InitAnswersForTest();
                break;
            case QuestionType.ShelfTest:
                InitAnswersForShelfTest();
                break;
            default:
                InitAnswersForTest();
                break;
        }            
        _typeAudio?.Play();
        //Debug.Log("_typeAudio Play");
    }

    private void FinishTypeTextCallBack()
    {
        _typeAudio?.Stop();
    }

    private void InitAnswersForTest()
    {
        List<Answer> answers = _questionsCurrentLevel[_currentQuestionIndex].GetAnswerList();

        if (answers.Count != _shelvesForCheck.Count)
            return;

        for (int i = 0; i < answers.Count; i++)
        {
            Answer answer = answers[i];
            Shelf shelf = _shelvesForCheck[i];

            Vector3 vectorPosition = shelf.gameObject.transform.position;
            vectorPosition.z = -0.02f;

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
        Vector3 vectorPosition = new Vector3(-_answers.Count - 2f, -1, -0.02f);
        //answers.Shuffle();
        foreach (var answer in answers)
        { 
            GameObject answerPrefab = Instantiate(_answerPrefab, vectorPosition, Quaternion.identity);
            SetAnswerDrag(answerPrefab, answer, answerPrefab.GetComponent<AnimationExecuter>());
            _answers.Add(answerPrefab);
            AnswerSurface answerSurface = answerPrefab?.GetComponent<AnswerSurface>();
            answerSurface.SetAnswer(answer, true);
            if (answerSurface != null) 
                _shelfRawAnswers.AddAnswerToShelf(answerSurface);
        }
        StartCoroutine(SetOpenOnStartRightAnswer());
    }

    private void InitAnswersForShelfTest()
    {
        List<Answer> answers = _questionsCurrentLevel[_currentQuestionIndex].GetAnswerList();
        List<Answer> answersForTestCheck = new List<Answer>();
        Vector3 vectorPosition = new Vector3(-_answers.Count - 2f, -1, -0.02f);
        //answers.Shuffle();
        foreach (var answer in answers)
        {
            if (answer.IsOpenOnStart)
            {
                GameObject answerPrefab = Instantiate(_answerReadyPrefab, vectorPosition, Quaternion.identity);
                SetAnswerDrag(answerPrefab, answer, answerPrefab.GetComponent<AnimationExecuter>());
                _answers.Add(answerPrefab);
                AnswerSurface answerSurface = answerPrefab?.GetComponent<AnswerSurface>();
                answerSurface.SetAnswer(answer, false);
                if (answerSurface != null)
                    _shelfRawAnswers.AddAnswerToShelf(answerSurface);
            }
            else
                answersForTestCheck.Add(answer);
        }

        _imageCheckerShelfTest.SetTestFromAnswers(answersForTestCheck, ClickImageTest);

        StartCoroutine(SetOpenOnStartRightAnswer());
    }

    #endregion

    #region Actions

    private void SetAnswerDrag(GameObject answerPrefab, Answer answer, AnimationExecuter animationExecuter)
    {
        AnswerSurface answerSurface = answerPrefab.GetComponent<AnswerSurface>();
        answerSurface.SetTitle(answer.Title);   
    }

    public void RemoveFromShelf(Transform transformTouchDown)
    {
        AnswerSurface answerSurface = transformTouchDown?.GetComponent<AnswerSurface>();
        if (answerSurface != null)
        {
            RemoveAnswerFromShelf(answerSurface);
            //StartCoroutine(RemoveAnswerFromShelfAfterDelay(answerSurface));
        }
    }

    public void CheckAnswerAfterDrop(Transform transform, bool isClick)
    {
        AnswerSurface answerSurface = transform.GetComponent<AnswerSurface>();
        if (answerSurface == null)
            return;
        if (_questionsCurrentLevel[_currentQuestionIndex].IsSingleRightAnswer)
            CheckSingleAnswerAfterDrop(answerSurface, isClick);
        else
            CheckMultiAnswerAfterDrop(answerSurface, isClick);
        _buttonCheck.SetActive(true);
        _buttonCheckDisabled.SetActive(false);
    }

    private void CheckMultiAnswerAfterDrop(AnswerSurface answerSurface, bool isClick)
    {
        bool isInsideAnyShelf = false;
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (_shelvesForCheck[i].IsAnswerInsideShelfBorders(answerSurface) && _shelvesForCheck[i].IsEnabled)
            {
                if (isClick)
                    _shelfRawAnswers.AddAnswerToShelfByDrag(answerSurface);
                else
                {
                    _shelvesForCheck[i].AddAnswerToShelfByDrag(answerSurface);
                    _shelfDefault = _shelvesForCheck[i];
                }
                isInsideAnyShelf = true;
                break;
            }
        }

        if (!isInsideAnyShelf)
        {
            if (isClick)
                AddAnswerToDefaultShelfAfterClick(answerSurface);
            else
                _shelfRawAnswers.AddAnswerToShelfByDrag(answerSurface);
        }
        CheckAllShelvesOnComplete();
    }

    private void AddAnswerToDefaultShelfAfterClick(AnswerSurface answerSurface)
    {
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (!_shelvesForCheck[i].GetIsShelfFull(answerSurface) && _shelvesForCheck[i].IsEnabled && _shelvesForCheck[i].Equals(_shelfDefault))
            {
                _shelvesForCheck[i].AddAnswerToShelfByDrag(answerSurface, true);
                //Debug.Log("CheckShelfForRightCompleted: " +
                //CheckShelfForRightCompleted(_shelvesForCheck[i], i);
                return;
            }
        }

        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (!_shelvesForCheck[i].GetIsShelfFull(answerSurface) && _shelvesForCheck[i].IsEnabled)
            {
                _shelvesForCheck[i].AddAnswerToShelfByDrag(answerSurface, true);
                //Debug.Log("CheckShelfForRightCompleted-Click: " +
                //CheckShelfForRightCompleted(_shelvesForCheck[i], i);
                break;
            }
        }
    }

    private void CheckSingleAnswerAfterDrop(AnswerSurface answerSurface, bool isClick)
    {   
        bool isInsideAnyShelf = false;
        foreach (Shelf shelf in _shelvesForCheck)
        {
            if (shelf.IsAnswerInsideShelfBorders(answerSurface)
                && !shelf.Equals(_shelfRawAnswers))
            {
                MoveAllAnswersToOtherShelf(shelf, _shelfRawAnswers);

                if (isClick)
                    _shelfRawAnswers.AddAnswerToShelfByDrag(answerSurface);
                else
                    shelf.AddAnswerToShelfByDrag(answerSurface);


                isInsideAnyShelf = true;
                break;
            }
        }

        if (!isInsideAnyShelf)
        {
            foreach (Shelf shelf in _shelvesForCheck)
            {
                MoveAllAnswersToOtherShelf(shelf, _shelfRawAnswers);
                break;
            }
            if (isClick)
                AddAnswerToDefaultShelfAfterClick(answerSurface);
            else
                _shelfRawAnswers.AddAnswerToShelfByDrag(answerSurface);
        }
        //CheckAllShelvesOnComplete();
    }

    private void MoveAllAnswersToOtherShelf(Shelf source, Shelf destination)
    {
        foreach (AnswerSurface answerSurface in source.GetAnswerList())
            destination.AddAnswerToShelf(answerSurface);
        
        source.GetAnswerList().Clear();
    }

    private void RemoveAnswerFromShelf(AnswerSurface answerSurface)
    {
        foreach (Shelf shelf in _shelvesForCheck)
        {
            if (shelf.GetAnswerList().Contains(answerSurface))
            {
                shelf.RemoveAnswerFromShelf(answerSurface);
                break;
            }
        }
        _shelfRawAnswers?.RemoveAnswerFromShelf(answerSurface);
    }

    private void CheckAllShelvesOnComplete()
    {
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
                CheckShelfForRightCompleted(_shelvesForCheck[i], i);
        }
    }

    private bool CheckShelfForRightCompleted(Shelf shelf, int rowIndex)
    {
        bool isRight = shelf.IsRightAnswersInShelf2(_questionsCurrentLevel[_currentQuestionIndex], rowIndex);
        if (isRight)
            shelf.SetCompleted();
        return isRight;
    }

    public void ClickCheckAnswerForQuestion()
    {
        //_clickAudio?.Play();
        Vibration.VibratePop();
        StopAnimationTextType();
        StopAllCoroutines();
        bool isRight = false;
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
                isRight = CheckShelfForRightCompleted(_shelvesForCheck[i], i);
            //else if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Test)
            //    isRight = _questionsCurrentLevel[_currentQuestionIndex].GetAnswerList()[i].IsRight == _shelvesForCheck[i].GetTestShelfChecker();
            if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf
                && !isRight)
                _shelvesForCheck[i].SetWrongCompleted();
            //if (!isRight)
            //    break;
        }
        if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Image
            ||
            _questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Test)
            isRight = _imageCheckerTest.GetIsRight();
        else if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.ShelfTest)
            isRight = _imageCheckerShelfTest.GetIsRight();

        //Debug.Log(isRight);
        if (isRight)
        {
            _questionsCurrentLevel[_currentQuestionIndex].IsPassed = isRight;
            _rightAnsweredCount++;
            ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
            //Debug.Log(_questionsCurrentLevel[_currentQuestionIndex].Score);
            //Debug.Log(_currentQuestionIndex);
            int points = ComonFunctions.GetScoreForLevel(_questionsCurrentLevel[_currentQuestionIndex].Score, buttonData.passCount, (ETypeLevel)buttonData.typeLevel);
            AddEarnedPoints(points);
        }

        StartCoroutine(SetRightAnswerOnScreen(isRight));
    }

    private IEnumerator SetOpenOnStartRightAnswer()
    {
        if (_questionsCurrentLevel == null || _currentQuestionIndex >= _questionsCurrentLevel.Count)
            yield break;

        if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType != QuestionType.Shelf
            &&
            _questionsCurrentLevel[_currentQuestionIndex].QuestionType != QuestionType.ShelfTest)
            yield break;

        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            List<AnswerSurface> answerSurfacesUpdated = _shelvesForCheck[i].GetAnswerList();
            List<AnswerSurface> answerSurfacesList = _shelfRawAnswers.GetAnswerList();
            foreach (AnswerSurface answerSurface in answerSurfacesList)
            {
                if (answerSurface.GetAnswer().IsOpenOnStart && answerSurface.GetAnswer().PositionRowIndex == i)
                {
                    if (answerSurface.GetAnswer().PositionCellIndex >= answerSurfacesUpdated.Count)
                        answerSurfacesUpdated.Add(answerSurface);
                    else
                        answerSurfacesUpdated.Insert(answerSurface.GetAnswer().PositionCellIndex, answerSurface);
                }
                if (!answerSurface.GetAnswer().IsOpenOnStart)
                    answerSurface.GetAnswer().IsEnabled = true;
            }
            if (answerSurfacesUpdated.Count > 0)
            {
                _shelvesForCheck[i].AddAnswerToShelfOnRightPlace(answerSurfacesUpdated);
                
                foreach (var item in answerSurfacesUpdated)
                {
                    _shelfRawAnswers.RemoveAnswerFromShelf(item);
                    if (!item.GetAnswer().IsOpenOnStart)
                        item.GetAnswer().IsEnabled = true;
                }
                _shelvesForCheck[i].IsEnabled = false;
            }
        }
        yield return new WaitForSeconds(0.01f);
        _shelfRawAnswers.GetAnswerList().Shuffle();
        _shelfRawAnswers.ReBuildBasePosition();
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
                CheckShelfForRightCompleted(_shelvesForCheck[i], i);
        }
        _shelfRawAnswers.SetFirstAnswerForRawShelfCompleted();
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            List<Answer> answers = _questionsCurrentLevel[_currentQuestionIndex].GetRigthAnswerList();
            int countAnswers = 0;
            foreach (var item in answers)
            {
                if (item.PositionRowIndex == i)
                    countAnswers++;
            }
            _shelvesForCheck[i].SetCountAnswerToBeOnShelf(countAnswers);
            _shelvesForCheck[i].SetHint();
        }
        yield return new WaitForSeconds(0.05f);
    }

    private IEnumerator SetRightAnswerOnScreen(bool isRight)
    {
        if (_questionsCurrentLevel == null || _currentQuestionIndex >= _questionsCurrentLevel.Count)
            yield break;
        
        string strTitle;
        if (isRight)
        {
            _OKAnswerAudio.Play();
            strTitle = LangAsset.GetValueByKey("PerfectDone");
        }
        else
        {
            _wrongAnswerAudio.Play();
            strTitle = LangAsset.GetValueByKey("DoneErrors");
        }
        _resultPanelScript.SetActive(true);
        _resultPanelScript.GetComponent<ResultPanel>().ShowPanel(isRight, strTitle, CallBackFromResultPanel);
        yield return new WaitForSeconds(0.5f);
        if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType != QuestionType.Shelf)
            yield break;

        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            List<AnswerSurface> answerSurfacesListForRemove = _shelvesForCheck[i].GetAnswerList();
            for (int j = answerSurfacesListForRemove.Count - 1; j >= 0; j--)
            {
                Answer answer = answerSurfacesListForRemove[j].GetAnswer();
                if (!answer.IsOpenOnStart && _shelvesForCheck[i].IsEnabled && (!answer.IsRight || answer.PositionRowIndex != i || answer.PositionCellIndex != j))
                {
                    var movingAnswerSurface = answerSurfacesListForRemove[j];
                    RemoveAnswerFromShelf(answerSurfacesListForRemove[j]);
                    _shelfRawAnswers.AddAnswerToShelfByDrag(movingAnswerSurface);
                }
            }            
        }
        yield return new WaitForSeconds(0.35f);

        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            List<AnswerSurface> answerSurfacesUpdated = _shelvesForCheck[i].GetAnswerList();
            List<AnswerSurface> answerSurfacesList = _shelfRawAnswers.GetAnswerList();
            foreach (AnswerSurface answerSurface in answerSurfacesList)
            {
                if (answerSurface.GetAnswer().IsRight && answerSurface.GetAnswer().PositionRowIndex == i && !answerSurface.GetAnswer().IsOpenOnStart)
                {
                    if (answerSurface.GetAnswer().PositionCellIndex >= answerSurfacesUpdated.Count)
                        answerSurfacesUpdated.Add(answerSurface);
                    else
                        answerSurfacesUpdated.Insert(answerSurface.GetAnswer().PositionCellIndex, answerSurface);
                }
            }
            _shelvesForCheck[i].AddAnswerToShelfOnRightPlace(answerSurfacesUpdated);
        }
        yield return new WaitForSeconds(0.5f);
    }

    private void CallBackFromResultPanel()
    {
        //_resultPanelScript?.gameObject?.SetActive(false);
        StopAllCoroutines();
        //SetEnabledAnswersOnShelvesForRepeatLevelAfterErrors();
        DestroyQuestionObjects();
        Debug.Log(_currentQuestionIndex);
        _currentQuestionIndex++;
        for (int i = _currentQuestionIndex; i < _questionsCurrentLevel.Count; i++)
        {
            if (!_questionsCurrentLevel[i].IsPassed)
                break;
            else
                _currentQuestionIndex++;
        }
        Debug.Log(_currentQuestionIndex);
        bool isLastQuestion = _currentQuestionIndex >= _questionsCurrentLevel.Count;
        Debug.Log(isLastQuestion);
        if (!isLastQuestion)
            InitQuestion();
        else
            _imageCheckerTest.gameObject.SetActive(false);

        SetLevelEarnedPoints(_rightAnsweredCount, _questionsCurrentLevel.Count);

        if (isLastQuestion) {
            if (CheckIsLevelCompleted())
                SetLevelCompleted();
            else
            {
                ShowWindowToRepeatErrorQuestions();
                //SetEnabledAnswersOnShelvesForRepeatLevelAfterErrors();
                InitQuestion();
            }
        }
    }

    private void SetEnabledAnswersOnShelvesForRepeatLevelAfterErrors()
    {
        for (int i = 0; i < _shelvesForCheck.Count; i++)
        {
            if (_questionsCurrentLevel[_currentQuestionIndex].QuestionType == QuestionType.Shelf)
                _shelvesForCheck[i].SetBackWrongCompleted();
        }
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

        //for (int i = _answersMock.Count - 1; i >= 0; i--)
        //{
        //    Destroy(_answersMock[i].gameObject);
        //}
        ////Destroy(_currentQuestionSurface.gameObject);
        _shelvesForCheck.Clear();
        _answers.Clear();
        //_answersMock.Clear();
    }

    private void SetLevelEarnedPoints(int rightAnsweredQuestion, int totalQuestionCount)
    {
        if (totalQuestionCount == 0)
            return;

        _canvasController.SetLevelProgress((float)rightAnsweredQuestion / totalQuestionCount);
    }

    private void AddEarnedPoints(int valuePoints)
    {
        //_scoreValue += valuePoints;
        //_scoreValueText.text = _scoreValue.ToString();
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
        _buttonCheck.SetActive(true);
        _buttonCheckDisabled.SetActive(false);
    }

    private void ClickImageTest()
    {
        //_clickAudio?.Play();
        Vibration.VibratePop();
        Debug.Log("ClickImageTest");
        _buttonCheck.SetActive(true);
        _buttonCheckDisabled.SetActive(false);
    }

    public void StopAnimationTextType()
    {
        //_clickAudio?.Play();
        Vibration.VibratePop();
        _questionText?.GetComponent<TextAnimation>()?.ClickButtonFinishReadingByUser();
        _typeAudio?.Stop();
    }

    private bool CheckIsLevelCompleted()
    {
        for (int i = 0; i < _questionsCurrentLevel.Count; i++)
        {
            if (!_questionsCurrentLevel[i].IsPassed)
            {
                _currentQuestionIndex = i;
                return false;
            }
        }
        return true;
    }

    private void SetLevelCompleted()
    {
        if (_currentQuestionIndex >= _questionsCurrentLevel.Count)
        {
            ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
            int score = ComonFunctions.GetScoreForLevel(buttonData.score, buttonData.passCount, (ETypeLevel)buttonData.typeLevel);
            PlayerPrefs.SetInt("AddedScore", score);
            int totalScore = UserData.Score + score;
            UserData.SetScore(totalScore);
            int errorCount = PlayerPrefs.GetInt("ErrorCount", 0); ;
            int starCount = 0;
            Debug.Log("STAR______________");
            Debug.Log(_secondsCountOnStart);
            if(errorCount <= 0)
                starCount = ComonFunctions.GetStarCountAfterLevelPass(_secondsCountOnStart, _secondsCountToLeft, buttonData.typeLevel);
            Debug.Log(starCount);
            Debug.Log(_secondsCountToLeft);
            buttonData.passCount++;
            if (buttonData.typeLevel == (int)ETypeLevel.simple)
                buttonData.activeStarsCount += starCount;
            else
                buttonData.activeStarsCount = starCount;
            if (buttonData.activeStarsCount > 3)
                buttonData.activeStarsCount = 3;

            bool isPassed = false;
            int currentLevel = Settings.Current_ButtonOnMapID;

            PlayerPrefs.SetFloat("PassSeconds", _secondsCountToLeft);
            PlayerPrefs.SetInt("PassQuestionCount", GetQuestionCountCurrentQuestionList());

            if (buttonData.passCount > 2)
            {
                Settings.Current_ButtonOnMapID++;
                DataLoader.SaveCurrentLevel();
                isPassed = true;
            }
            DataLoader.SaveLevelResults(currentLevel, !isPassed, isPassed, buttonData.activeStarsCount, buttonData.passCount);
            
            if (isPassed) // set next level = true -- isActive, to show on map
                DataLoader.SaveLevelResults(Settings.Current_ButtonOnMapID, true, false, 0, 0);
            Debug.Log("UpdateUser: " + UserData.Score);
            if (UserData.UserID != "")
                StartCoroutine(ComonFunctions.Instance.UpdateUser(UserData.UserID, UserData.UserName, UserData.UserEmail, UserData.UserPassword, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID, UserData.Score));
            PlayerPrefs.SetInt("ErrorCount", 0);
            ActionLevelCompleted.Invoke();
        }
    }

    private IEnumerator TimerCountDown(int secondsTillFinish)
    {
        _secondsCountToLeft = secondsTillFinish;
        _scoreValueText.text = ComonFunctions.GetMinetsSecondsFromSeconds(_secondsCountToLeft);
        while (_secondsCountToLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            _secondsCountToLeft--;
            _scoreValueText.text = ComonFunctions.GetMinetsSecondsFromSeconds(_secondsCountToLeft);
        }
        if (_secondsCountToLeft < 0)
            _secondsCountToLeft = 0;
        _scoreValueText.text = ComonFunctions.GetMinetsSecondsFromSeconds(_secondsCountToLeft);
        StopQuizByTimer();
        yield break;
    }

    private void StopQuizByTimer()
    {
        //Scene scene = SceneManager.GetSceneByName("WindowTimerStopScene");
        //if (scene.isLoaded)
        //    return;
        PlayerPrefs.SetString("SceneToLoad", "MapScene");
        SceneManager.LoadScene("WindowTimerStopScene", LoadSceneMode.Single);
        StopAllCoroutines();
    }

    private void ShowWindowToRepeatErrorQuestions()
    {
        StopAllCoroutines();
        Scene scene = SceneManager.GetSceneByName("WindowRepeatErrorScene");
        if (scene.isLoaded)
            return;
        int errorCount = PlayerPrefs.GetInt("ErrorCount", 0);
        errorCount++;
        PlayerPrefs.SetInt("ErrorCount", errorCount);
        SceneManager.LoadScene("WindowRepeatErrorScene", LoadSceneMode.Additive);
        
    }

    public void ClickCatImage()
    {
        _clickCatImageAudio.Play();
    }

    #endregion

    public static int GetSecondsForCurrentQuestionList()
    {
        int secondsCount;
        Debug.Log(_questionsCurrentLevel.Count);
        Debug.Log(_currentQuestionIndex);
        if (_questionsCurrentLevel[_currentQuestionIndex].TypeLevel == ETypeLevel.additional)
            secondsCount = 120;
        else if (_questionsCurrentLevel[_currentQuestionIndex].TypeLevel == ETypeLevel.mission1)
            secondsCount = 100;
        else if (_questionsCurrentLevel[_currentQuestionIndex].TypeLevel == ETypeLevel.mission2)
            secondsCount = 90;
        else if (_questionsCurrentLevel[_currentQuestionIndex].TypeLevel == ETypeLevel.final)
            secondsCount = 60;
        else
            secondsCount = 300;
        return secondsCount;
    }

    public static int GetQuestionCountCurrentQuestionList()
    {
        return _questionsCurrentLevel.Count;
    }
}
