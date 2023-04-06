using System.Collections.Generic;

public class Answer: Information
{
    private bool _isRight = false;
    private int _score = 0;
    private bool _isPositionDependent = false;
    private int _positionRowIndex = 0;
    private int _positionCellIndex = 0;

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

    public bool IsPositionDependent
    {
        get
        {
            return _isPositionDependent;
        }
        set
        {
            _isPositionDependent = value;
        }
    }

    public int PositionRowIndex
    {
        get
        {
            return _positionRowIndex;
        }
        set
        {
            _positionRowIndex = value;
        }
    }

    public int PositionCellIndex
    {
        get
        {
            return _positionCellIndex;
        }
        set
        {
            _positionCellIndex = value;
        }
    }

    public bool IsRightInputValuesForShelf(int rowIndex, int cellIndex)
    {
        if (!_isPositionDependent)
            return _isRight;
        else
            return _isRight
                && rowIndex == _positionRowIndex
                && cellIndex == _positionCellIndex;
    }

    public bool IsRightInputValuesForTest(bool checkValue)
    {
        return _isRight == checkValue;
    }
}
