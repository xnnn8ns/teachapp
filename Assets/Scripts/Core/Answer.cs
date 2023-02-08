using System.Collections.Generic;

public class Answer
{
    private string _answerTitle = "";
    private bool _isRight = false;
    private int _score = 0;

    public string Title
    {
        get
        {
            return _answerTitle;
        }
        set
        {
            _answerTitle = value;
        }
    }

    public bool IsRight
    {
        get
        {
            return _isRight;
        }
        set
        {
            _isRight = value;
        }
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
}
