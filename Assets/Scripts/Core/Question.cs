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
        int count = 0;
        foreach (var item in _answerList)
        {
            if (item.IsRight)
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
                continue;
            else
                return false;
        }
        return true;
    }
}
