using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Question : Information
{
    public static List<Question> QuestionsList = new List<Question>();

    private int _level = 1;
    private int _step = 1;
    private int _topic = 1;
    private int _countShelves = 1;
    private bool _isSingleRightAnswer = false;
    private int _score = 0;
    private int _difficulty = 1;
    private QuestionType _questionType = QuestionType.Shelf;
    private List<Answer> _answerList = new List<Answer>();
    private bool _isPassed = false;
    private ETypeLevel _ETypeLevel = ETypeLevel.simple;

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

    public List<Answer> GetRigthAnswerList()
    {
        List<Answer> answers = new List<Answer>();

        foreach (var item in _answerList)
        {
            if (item.IsRight)
                answers.Add(item);
        }
        return answers;
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

    public int Topic
    {
        get
        {
            return _topic;
        }
        set
        {
            _topic = value;
        }
    }

    public int Step
    {
        get
        {
            return _step;
        }
        set
        {
            _step = value;
        }
    }

    public bool IsPassed
    {
        get
        {
            return _isPassed;
        }
        set
        {
            _isPassed = value;
        }
    }

    public int Difficulty
    {
        get
        {
            return _difficulty;
        }
        set
        {
            _difficulty = value;
        }
    }

    public ETypeLevel TypeLevel
    {
        get
        {
            return _ETypeLevel;
        }
        set
        {
            _ETypeLevel = value;
        }
    }

    //public static List<Question> GetQuestionListForLevel2(int buttonOnMapID, int stepID, ETypeLevel eTypeLevel)
    //{
    //    List<Question> questionsLevel = new List<Question>();
    //    int topicID = Settings.GetTopicFromButtonOnMapID(buttonOnMapID);
    //    int levelID = Settings.GetLevelFromButtonOnMapID(buttonOnMapID);
    //    foreach (var question in Question.QuestionsList)
    //    {
    //        if (question.Topic == topicID
    //            &&
    //            question.Level == levelID
    //            &&
    //            question.Step == stepID + 1)
    //            questionsLevel.Add(question);
    //    }
    //    foreach (var item in questionsLevel)
    //    {
    //        item.TypeLevel = eTypeLevel;
    //    }
    //    return questionsLevel;
    //}

    public static List<Question> GetQuestionListForLevel(int buttonOnMapID, int stepID, ETypeLevel eTypeLevel)
    {
        List<Question> qList;
        List<Question> questionsTopic = GetQuestionListForTopic(buttonOnMapID);
        int levelID = Settings.GetLevelFromButtonOnMapID(buttonOnMapID);
        qList = GetQuestionListForLevelFromTopic(questionsTopic, levelID);

        AddRandomQuestions(qList, levelID, stepID);

        foreach (var item in qList)
            item.TypeLevel = eTypeLevel;
        
        return qList;
    }

    private static void AddRandomQuestions(List<Question> qList, int levelID, int stepID)
    {
        qList.Insert(0, AlgorithmTestContriller.GetQuestionFromAlgo(Settings.Current_Topic, levelID, stepID));
    }

    private static void AddRandomQuestions2(List<Question> qList, int levelID, int stepID)
    {
        qList.Insert(1, AlgorithmTestContriller.Test_0_KeyWords(Settings.Current_Topic, levelID, stepID));
        qList.Insert(3, AlgorithmTestContriller.Test_1_KeyOperators(Settings.Current_Topic, levelID, stepID));

        Random random = new Random();
        int rand = random.Next(0, 10);
        switch (rand)
        {
            case 0:
                qList.Insert(0, AlgorithmTestContriller.Algo0(Settings.Current_Topic, levelID, stepID));
                break;
            case 1:
                qList.Insert(0, AlgorithmTestContriller.Algo1(Settings.Current_Topic, levelID, stepID));
                break;
            case 2:
                qList.Insert(0, AlgorithmTestContriller.Algo2(Settings.Current_Topic, levelID, stepID));
                break;
            case 3:
                qList.Insert(0, AlgorithmTestContriller.Algo3(Settings.Current_Topic, levelID, stepID));
                break;
            case 4:
                qList.Insert(0, AlgorithmTestContriller.Algo4(Settings.Current_Topic, levelID, stepID));
                break;
            case 5:
                qList.Insert(0, AlgorithmTestContriller.Algo5(Settings.Current_Topic, levelID, stepID));
                break;
            case 6:
                qList.Insert(0, AlgorithmTestContriller.Algo6(Settings.Current_Topic, levelID, stepID));
                break;
            case 7:
                qList.Insert(0, AlgorithmTestContriller.Algo7(Settings.Current_Topic, levelID, stepID));
                break;
            case 8:
                qList.Insert(0, AlgorithmTestContriller.Algo8(Settings.Current_Topic, levelID, stepID));
                break;
            case 9:
                qList.Insert(0, AlgorithmTestContriller.Algo9(Settings.Current_Topic, levelID, stepID));
                break;
            default:
                qList.Insert(0, AlgorithmTestContriller.Algo0(Settings.Current_Topic, levelID, stepID));
                break;
        }

        //qList.Insert(0, AlgorithmTestContriller.Algo0(Settings.Current_Topic, levelID, stepID));
        //qList.Insert(0, AlgorithmTestContriller.Algo2(Settings.Current_Topic, levelID, stepID));
        //qList.Insert(0, AlgorithmTestContriller.Algo3(Settings.Current_Topic, levelID, stepID));
        //qList.Insert(0, AlgorithmTestContriller.Algo4(Settings.Current_Topic, levelID, stepID));
        //qList.Insert(0, AlgorithmTestContriller.Algo5(Settings.Current_Topic, levelID, stepID));
        //qList.Insert(0, AlgorithmTestContriller.Algo6(Settings.Current_Topic, levelID, stepID));
        //qList.Insert(0, AlgorithmTestContriller.Algo7(Settings.Current_Topic, levelID, stepID));
        //qList.Insert(0, AlgorithmTestContriller.Algo8(Settings.Current_Topic, levelID, stepID));
        //qList.Insert(0, AlgorithmTestContriller.Algo9(Settings.Current_Topic, levelID, stepID));
    } 

    private static List<Question> GetQuestionListForTopic(int buttonOnMapID)
    {
        int topicID = Settings.GetTopicFromButtonOnMapID(buttonOnMapID);
        List<Question> qTopicList = new List<Question>();
        foreach (var q in Question.QuestionsList)
        {
            if (q.Topic <= topicID)
                qTopicList.Add(q);
        }
        qTopicList = qTopicList.OrderBy(x => x.Score).ToList();
        return qTopicList;
    }

    private static List<Question> GetQuestionListForLevelFromTopic(List<Question> qTopic, int levelID)
    {
        List<Question> list = new List<Question>();
        List<Question> listTest = new List<Question>();
        List<Question> listTask = new List<Question>();

        if (qTopic.Count <= 0)
            return list;

        int countInLevel = qTopic.Count * levelID / 10;
        int countTest = countInLevel;
        foreach (var q in qTopic)
        {
            if(q.QuestionType == QuestionType.Test && countTest > 0)
            {
                listTest.Add(q);
                countTest--;
            }
            if (countTest <= 0)
                break;
        }
        countTest = countInLevel;
        foreach (var q in qTopic)
        {
            if (q.QuestionType == QuestionType.Shelf && countTest > 0)
            {
                listTask.Add(q);
                countTest--;
            }
            if (countTest <= 0)
                break;
        }

        int countLTest = listTest.Count;
        int thirtyPercent = countLTest / 3;
        int sixtyPercent = countLTest * 2 / 3;
        List<Question> thirtyPercentTestList = listTest.GetRange(0, thirtyPercent);
        List<Question> sixtyFivePercentTestList = listTest.GetRange(thirtyPercent, sixtyPercent - thirtyPercent);
        List<Question> hundredFivePercentTestList = listTest.GetRange(sixtyPercent, countLTest - sixtyPercent);


        int countLTask = listTask.Count;
        thirtyPercent = countLTask / 3;
        sixtyPercent = countLTask * 2 / 3;
        //List<Question> thirtyPercentTaskList = listTask.GetRange(0, thirtyPercent);
        //List<Question> sixtyFivePercentTaskList = listTask.GetRange(thirtyPercent, sixtyPercent - thirtyPercent);
        List<Question> sixtyFivePercentTaskList = listTask.GetRange(0, sixtyPercent - thirtyPercent);
        List<Question> hundredFivePercentTaskList = listTask.GetRange(sixtyPercent, countLTask - sixtyPercent);

        Random rand = new Random();

        int indexRand;
        int countToAdd = 1; 
        while (countToAdd > 0)
        {
            indexRand = rand.Next(0, thirtyPercentTestList.Count);
            list.Add(thirtyPercentTestList[indexRand]);
            countToAdd--;
        }

        countToAdd = 1;
        
        while (countToAdd > 0)
        {
            if (countToAdd % 2 != 0)
            {
                indexRand = rand.Next(0, sixtyFivePercentTaskList.Count);
                list.Add(sixtyFivePercentTaskList[indexRand]);
            }
            else
            {
                indexRand = rand.Next(0, sixtyFivePercentTestList.Count);
                list.Add(sixtyFivePercentTestList[indexRand]);
            }
            countToAdd--;
        }

        countToAdd = 1;
        while (countToAdd > 0)
        {
            if (countToAdd == 1)
            {
                indexRand = rand.Next(0, hundredFivePercentTaskList.Count);
                list.Add(hundredFivePercentTaskList[indexRand]);
            }
            else
            {
                indexRand = rand.Next(0, hundredFivePercentTestList.Count);
                list.Add(hundredFivePercentTestList[indexRand]);
            }
            countToAdd--;
        }

        return list;
    }
}