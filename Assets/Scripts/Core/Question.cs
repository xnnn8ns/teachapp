using System.Collections.Generic;

public abstract class Question
{
    private string _questionTitle = "";
    private List<Answer> _answerList = new List<Answer>();

    public int GetAnswerCount()
    {
        return _answerList.Count;
    }

    public int GetCountRigthAnswers()
    {
        return 0;
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

    public string Title
    {
        get
        {
            return _questionTitle;
        }
        set
        {
            _questionTitle = value;
        }
    }

    public List<Answer> GetAnswerList()
    {
        return _answerList;
    }
}
