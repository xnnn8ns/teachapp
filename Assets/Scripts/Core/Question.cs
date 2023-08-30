using System.Collections.Generic;

public abstract class Question : Information
{
    public static List<Question> QuestionsList = new List<Question>();

    private int _level = 1;
    private int _countShelves = 1;
    private bool _isSingleRightAnswer = false;
    private int _score = 0;
    private QuestionType _questionType = QuestionType.Shelf;
    private List<Answer> _answerList = new List<Answer>();

    public int GetAnswerCount()
    {
        return _answerList.Count;
    }

    public int GetCountRigthAnswers()
    {
        int count = 0;
        foreach (var item in _answerList)
        {
            if (item.IsRight)
                count++;
        }
        return count;
    }

    public int GetCountRigthAnswersForRowIndex(int rowIndex)
    {
        int count = 0;
        foreach (var item in _answerList)
        {
            if ((item.PositionRowIndex == rowIndex || !item.IsPositionRowDependent ) && item.IsRight)
                count++;
        }
        return count;
    }

    public List<int> GetRigthAnswers()
    {
        return new List<int>();
    }

    public bool IsRigthAnswerByIndex(int indexAnswerForCheck)
    {
        return false;
    }

    public void SetAnswerList(List<Answer> answerList)
    {
        _answerList = answerList;
    }

    public List<Answer> GetAnswerList()
    {
        return _answerList;
    }

    public bool IsRightAnswerForQuestion(List<Answer> answers)
    {
        if (answers.Count != GetCountRigthAnswers())
            return false;

        foreach (var item in answers)
        {
            if (item.IsRight)
            {
                continue;
            }
            else
                return false;
        }
        return true;
    }

    public int CountShelves
    {
        get
        {
            return _countShelves;
        }
        set
        {
            _countShelves = value;
        }
    }

    public bool IsSingleRightAnswer
    {
        get
        {
            return _isSingleRightAnswer;
        }
        set
        {
            _isSingleRightAnswer = value;
        }
    }

    public QuestionType QuestionType
    {
        get => _questionType;
        set => _questionType = value;
    }

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
        }
    }
}
