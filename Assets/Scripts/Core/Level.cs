using System.Collections;
using System.Collections.Generic;

public class Level
{
    public static List<Level> Levels = new List<Level>();

    private int _levelNumber = 1;
    private int _totalTime = 1;
    private int _totalScore = 1;
    private int _totalCount = 1;
    private List<Question> _questions = new List<Question>();

    public int LevelNumber
    {
        get
        {
            return _levelNumber;
        }
        set
        {
            _levelNumber = value;
        }
    }

    public int TotalCount
    {
        get
        {
            return _totalCount;
        }
        set
        {
            _totalCount = value;
        }
    }

    public int TotalTime
    {
        get
        {
            return _totalTime;
        }
        set
        {
            _totalTime = value;
        }
    }

    public int TotalScore
    {
        get
        {
            return _totalScore;
        }
        set
        {
            _totalScore = value;
        }
    }

}
