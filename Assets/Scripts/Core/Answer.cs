using System.Collections.Generic;

public class Answer: Information
{
    private bool _isRight = false;
    private int _score = 0;
    private bool _isPositionRowDependent = false;
    private bool _isPositionCellDependent = false;
    private int _positionRowIndex = 0;
    private int _positionCellIndex = 0;
    private bool _isOpenOnStart = false;
    private bool _isEnabled = true;

    public bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            _isEnabled = value;
        }
    }

    public bool IsOpenOnStart
    {
        get
        {
            return _isOpenOnStart;
        }
        set
        {
            _isOpenOnStart = value;
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

    public bool IsPositionRowDependent
    {
        get
        {
            return _isPositionRowDependent;
        }
        set
        {
            _isPositionRowDependent = value;
        }
    }

    public bool IsPositionCellDependent
    {
        get
        {
            return _isPositionCellDependent;
        }
        set
        {
            _isPositionCellDependent = value;
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
        if (!_isPositionRowDependent && !_isPositionCellDependent)
            return _isRight;
        else
            return _isRight
                && (rowIndex == _positionRowIndex || !_isPositionRowDependent) 
                && (cellIndex == _positionCellIndex || !_isPositionCellDependent);
    }

    public bool IsRightInputValuesForTest(bool checkValue)
    {
        return _isRight == checkValue;
    }
}
