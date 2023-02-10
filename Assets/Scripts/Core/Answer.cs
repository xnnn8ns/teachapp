using System.Collections.Generic;

public class Answer: Information
{
    private bool _isRight = false;
    private int _score = 0;

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
