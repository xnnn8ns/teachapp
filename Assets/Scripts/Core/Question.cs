using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

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

    public static List<Question> GetQuestionListForLevel(int topic, int buttonOnMapID, int passCount, ETypeLevel eTypeLevel)
    {
        UnityEngine.Debug.Log("GetQuestionListForLevel");
        UnityEngine.Debug.Log(topic);
        UnityEngine.Debug.Log(buttonOnMapID);
        List<Question> qList;
        List<Question> questionsTopic = GetQuestionListForTopic(buttonOnMapID);
        //UnityEngine.Debug.Log("levelID: " + levelID);
        int countQuiz = GetRequestCountQuestionsForTopic(topic);
        Tuple<bool, QuestionType> typeQuiz = GetQuestionTypeByButtonNumber(topic, buttonOnMapID);

        if(typeQuiz.Item1 == true)
            qList = GetQuestionListForLevelFromTopic(topic, questionsTopic, buttonOnMapID);
        else if (typeQuiz.Item2 == QuestionType.Test)
            qList = GetQuestionOnlyTestListForLevelFromTopic(topic, questionsTopic, buttonOnMapID);
        else if (typeQuiz.Item2 == QuestionType.ShelfTest)
            qList = new List<Question>();
        else
            qList = GetQuestionOnlyTaskListForLevelFromTopic(topic, questionsTopic, buttonOnMapID);

        if (qList.Count > countQuiz)
        {
            System.Random rand = new System.Random();
            while (qList.Count > countQuiz)
            {
                qList.RemoveAt(rand.Next(qList.Count-1));
            }
        }
        
        UnityEngine.Debug.Log("qList: " + qList.Count);
        //foreach (var item in qList)
        //    UnityEngine.Debug.Log(item.Score);
        //UnityEngine.Debug.Log(qList.Count);
        int maxCount = 100;
        while (qList.Count < countQuiz)
        {
            AddRandomQuestions(topic, qList, buttonOnMapID, passCount, countQuiz);
            maxCount--;
            if (maxCount < 0)
            {
                UnityEngine.Debug.Log("while endless break");
                break;
            }
        }
        //UnityEngine.Debug.Log(qList.Count);
        foreach (var item in qList)
            item.TypeLevel = eTypeLevel;
        UnityEngine.Debug.Log("qList.Count: " + qList.Count);
        return qList;
    }

    private static int GetRequestCountQuestionsForTopic(int topic)
    {
        
        if (topic <= 5)
            return topic + 1;
        else
            return topic + 2;
        //else if (topic <= 2)
        //    return 4;
        //else if (topic <= 4)
        //    return 5;
        //else if (topic <= 6)
        //    return 6;
        //else if (topic <= 7)
        //    return 7;
        //else if (topic <= 8)
        //    return 8;
        //else if (topic <= 9)
        //    return 9;
        //else
        //    return 10;
    }

    private static void AddRandomQuestions(int topic, List<Question> qList, int levelID, int stepID, int countQuiz)
    {
        //UnityEngine.Debug.Log(levelID);

        Tuple<bool, QuestionType> typeQuiz = GetQuestionTypeByButtonNumber(topic, levelID);
        if (typeQuiz.Item1 == true || typeQuiz.Item2 == QuestionType.Test)
        {
            if (qList.Count < countQuiz)
                qList.Insert(0, AlgorithmTestContriller.Test_0_KeyWords(topic, levelID, stepID));
            if (qList.Count < countQuiz)
                qList.Insert(0, AlgorithmTestContriller.Test_1_KeyOperators(topic, levelID, stepID));
            if (qList.Count < countQuiz)
                qList.Insert(0, AlgorithmTestContriller.Test_2_KeyBuildIns(topic, levelID, stepID));
        }
        if (qList.Count < countQuiz && (typeQuiz.Item1 == false || typeQuiz.Item2 != QuestionType.Test))
            qList.Insert(0, AlgorithmTestContriller.GetRandomShelfTestQuestionFromAlgo(topic, levelID, stepID));
        if (qList.Count < countQuiz && typeQuiz.Item1 == true)
            qList.Insert(0, AlgorithmTestContriller.GetRandomQuestionFromAlgo(topic, levelID, stepID));
        if (typeQuiz.Item1 == false && typeQuiz.Item2 == QuestionType.Test)
        {
            qList.Insert(0, AlgorithmTestContriller.GetRandomTestQuestionFromAlgo(topic, levelID, stepID));
        }

    }

    private static List<Question> GetQuestionListForTopic(int buttonOnMapID)
    {
        int topicID = Settings.GetTopicFromButtonOnMapID(buttonOnMapID);
        List<Question> qTopicList = new List<Question>();
        foreach (var q in QuestionsList)
        {
            if (q.Topic <= topicID)// && q.Topic >= topicID - 2)
            {
                //UnityEngine.Debug.Log(q.Title);
                qTopicList.Add(q);
            }
        }
        //UnityEngine.Debug.Log("topicID: " + topicID + " --- qTopicList.Count: " + qTopicList.Count);
        //UnityEngine.Debug.Log(qTopicList.Count);
        qTopicList = qTopicList.OrderBy(x => x.Topic).OrderBy(x => x.Level).ThenBy(x => x.Score).ToList();
        return qTopicList;
    }

    private static List<Question> GetQuestionOnlyTestListForLevelFromTopic(int topic, List<Question> qTopic, int levelID)
    {
        List<Question> list = new List<Question>();
        List<Question> listTest = new List<Question>();

        if (qTopic.Count <= 0)
            return list;

        int countInLevel = qTopic.Count * levelID / 12;
        int countTest = countInLevel;
        foreach (var q in qTopic)
        {
            if (q.QuestionType == QuestionType.Test && countTest > 0)
            {
                listTest.Add(q);
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
        List<Question> hundredPercentTestList = listTest.GetRange(sixtyPercent, countLTest - sixtyPercent);

        System.Random rand = new System.Random();

        //UnityEngine.Debug.Log(list.Count);
        int indexRand;
        int countToAdd = 2;
        //if (topic < 2)
        //    countToAdd = 2;
        while (countToAdd > 0 && thirtyPercentTestList.Count > 0)
        {
            UnityEngine.Debug.Log(thirtyPercentTestList.Count);
            indexRand = rand.Next(0, thirtyPercentTestList.Count);
            list.Add(thirtyPercentTestList[indexRand]);
            countToAdd--;
        }
        //UnityEngine.Debug.Log(list.Count);
        countToAdd = 2;
        //if (topic < 2)
        //    countToAdd = 0;
        while (countToAdd > 0)
        {
            indexRand = rand.Next(0, sixtyFivePercentTestList.Count);
            list.Add(sixtyFivePercentTestList[indexRand]);
            countToAdd--;
        }

        countToAdd = 2;
        //if (topic < 2)
        //    countToAdd = 0;
        while (countToAdd > 0 && hundredPercentTestList.Count > 0)
        {

            indexRand = rand.Next(0, hundredPercentTestList.Count);
            list.Add(hundredPercentTestList[indexRand]);
            countToAdd--;
        }
        //UnityEngine.Debug.Log(list.Count);
        return list;
    }

    private static List<Question> GetQuestionOnlyTaskListForLevelFromTopic(int topic, List<Question> qTopic, int levelID)
    {
        List<Question> list = new List<Question>();
        List<Question> listTask = new List<Question>();

        if (qTopic.Count <= 0)
            return list;

        int countInLevel = qTopic.Count * levelID / 12;
        int countTest = countInLevel;

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

        int countLTask = listTask.Count;
        int thirtyPercent = countLTask / 3;
        int sixtyPercent = countLTask * 2 / 3;
        List<Question> thirtyPercentTaskList = listTask.GetRange(0, thirtyPercent);
        List<Question> sixtyFivePercentTaskList = listTask.GetRange(thirtyPercent, sixtyPercent - thirtyPercent);
        //List<Question> sixtyFivePercentTaskList = listTask.GetRange(0, sixtyPercent - thirtyPercent);
        List<Question> hundredFivePercentTaskList = listTask.GetRange(sixtyPercent, countLTask - sixtyPercent);

        System.Random rand = new System.Random();


        Tuple<bool, QuestionType> typeQuiz = GetQuestionTypeByButtonNumber(topic, levelID);


        //UnityEngine.Debug.Log(list.Count);
        int indexRand;
        int countToAdd = 2;

        //    countToAdd = 0;
        while (countToAdd > 0 && sixtyFivePercentTaskList.Count > 0 && thirtyPercentTaskList.Count > 0)
        {
            if (topic > 2)
            {
                indexRand = rand.Next(0, sixtyFivePercentTaskList.Count);
                list.Add(sixtyFivePercentTaskList[indexRand]);
            }
            else
            {
                indexRand = rand.Next(0, thirtyPercentTaskList.Count);
                list.Add(thirtyPercentTaskList[indexRand]);
            }
            countToAdd--;
        }

        countToAdd = 1;
        if (topic < 2)
            countToAdd = 0;
        while (countToAdd > 0 && hundredFivePercentTaskList.Count > 0)
        {
            indexRand = rand.Next(0, hundredFivePercentTaskList.Count);
            list.Add(hundredFivePercentTaskList[indexRand]);
            countToAdd--;
        }
        //UnityEngine.Debug.Log(list.Count);
        return list;
    }

    private static List<Question> GetQuestionListForLevelFromTopic(int topic, List<Question> qTopic, int levelID)
    {
        List<Question> list = new List<Question>();
        List<Question> listTest = new List<Question>();
        List<Question> listTask = new List<Question>();

        if (qTopic.Count <= 0)
            return list;

        int countInLevel = qTopic.Count * levelID / 12;
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
        List<Question> thirtyPercentTaskList = listTask.GetRange(0, thirtyPercent);
        List<Question> sixtyFivePercentTaskList = listTask.GetRange(thirtyPercent, sixtyPercent - thirtyPercent);
        //List<Question> sixtyFivePercentTaskList = listTask.GetRange(0, sixtyPercent - thirtyPercent);
        List<Question> hundredFivePercentTaskList = listTask.GetRange(sixtyPercent, countLTask - sixtyPercent);

        System.Random rand = new System.Random();


        Tuple<bool, QuestionType> typeQuiz = GetQuestionTypeByButtonNumber(topic, levelID);


        //UnityEngine.Debug.Log(list.Count);
        int indexRand;
        int countToAdd = 1;
        if (topic < 2)
            countToAdd = 2;
        while (countToAdd > 0 && thirtyPercentTestList.Count > 0)
        {
            UnityEngine.Debug.Log(thirtyPercentTestList.Count);
            indexRand = rand.Next(0, thirtyPercentTestList.Count);
            list.Add(thirtyPercentTestList[indexRand]);
            countToAdd--;
        }
        //UnityEngine.Debug.Log(list.Count);
        countToAdd = 1;
        //if (topic < 2)
        //    countToAdd = 0;
        while (countToAdd > 0 && sixtyFivePercentTaskList.Count > 0 && thirtyPercentTaskList.Count > 0)
        {
            if (topic > 2 && countToAdd % 2 != 0)
            {
                indexRand = rand.Next(0, sixtyFivePercentTaskList.Count);
                list.Add(sixtyFivePercentTaskList[indexRand]);
            } else if (topic <= 2 && countToAdd % 2 != 0)
            {
                indexRand = rand.Next(0, thirtyPercentTaskList.Count);
                list.Add(thirtyPercentTaskList[indexRand]);
            }
            else
            {
                indexRand = rand.Next(0, sixtyFivePercentTestList.Count);
                list.Add(sixtyFivePercentTestList[indexRand]);
            }
            countToAdd--;
        }

        countToAdd = 1;
        if (topic < 2)
            countToAdd = 0;
        while (countToAdd > 0 && hundredFivePercentTaskList.Count > 0 && hundredFivePercentTestList.Count > 0)
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
        //UnityEngine.Debug.Log(list.Count);
        return list;
    }

    private static Tuple<bool, QuestionType> GetQuestionTypeByButtonNumber(int topic, int levelID)
    {
        if (topic > 1)
            levelID -= topic * 12;
        if (levelID >= 10)
            return new Tuple<bool, QuestionType>(true, QuestionType.Test);
        else if (levelID == 1 || levelID == 2 || levelID == 3)
            return new Tuple<bool, QuestionType>(false, QuestionType.Test);
        else if (levelID == 5 || levelID == 6)
            return new Tuple<bool, QuestionType>(false, QuestionType.ShelfTest);
        else
            return new Tuple<bool, QuestionType>(false, QuestionType.Shelf);

    }
}