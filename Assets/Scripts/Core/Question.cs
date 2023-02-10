using System.Collections.Generic;

public abstract class Question: Information
{
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

    public List<Answer> GetAnswerList()
    {
        return _answerList;
    }
}
