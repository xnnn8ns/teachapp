using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class AlgorithmTestContriller : MonoBehaviour
{
    private static List<string> _keyWordsForAlgo;
    private static List<KeyWord> _keyWordsForTestList = null;
    private static List<KeyWord> _keyOperatorsForTestList = null;
    private static List<AlgoInfo> _algoInfoList = null;
    private static List<string> _varNames;

    #region KeyMethods

    private static List<AlgoInfo> GetAlgoInfoList()
    {
        if (_algoInfoList == null)
        {
            _algoInfoList = new List<AlgoInfo>();
            ImportAlgoInfo();
        }
        return _algoInfoList;
    }

    private static List<string> GetKeyWordsForAlgo()
    {
        if (_keyWordsForAlgo == null)
        {
            _keyWordsForAlgo = new List<string>();
            ImportKeyWordsForAlgo();
        }
        return _keyWordsForAlgo;
    }

    private static List<KeyWord> GetKeyOperatorsForTest()
    {
        if (_keyOperatorsForTestList == null)
        {
            _keyOperatorsForTestList = new List<KeyWord>();
            ImportKeyOperatorsForTest();
        }
        return _keyOperatorsForTestList;
    }

    private static List<KeyWord> GetKeyWordsForTest()
    {
        if (_keyWordsForTestList == null)
        {
            _keyWordsForTestList = new List<KeyWord>();
            ImportKeyWordsForTest();
        }
        return _keyWordsForTestList;
    }

    private static List<string> GetVarNames()
    {
        if (_varNames == null)
        {
            _varNames = new List<string>();
            ImportVarNames();
        }
        return _varNames;
    }

    private static void ImportAlgoInfo()
    {
        GetAlgoInfoList().Clear();
        var dataset = Resources.Load<TextAsset>("algo_distribution");
        //Debug.Log(dataset);
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        
        for (var i = 1; i < splitDataset.Length; i++)
        {
            //Debug.Log(splitDataset[i]);
            var splitRow = splitDataset[i].Split(new char[] { ';' });
            //Debug.Log(splitRow);
            AlgoInfo algoInfo = new AlgoInfo();
            algoInfo.Title = splitRow[0];
            algoInfo.StartTopic = int.Parse(splitRow[1]);
            algoInfo.StartLevel = int.Parse(splitRow[2]);
            algoInfo.FinishTopic = int.Parse(splitRow[3]);
            algoInfo.FinishLevel = int.Parse(splitRow[4]);
            algoInfo.QuestionType = int.Parse(splitRow[5]);
            GetAlgoInfoList().Add(algoInfo);
        }
    }

    private static void ImportKeyOperatorsForTest()
    {
        GetKeyOperatorsForTest().Clear();
        var dataset = Resources.Load<TextAsset>("keyoperators_for_test");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            var splitRow = splitDataset[i].Split(new char[] { ';' });
            KeyWord keyWord = new KeyWord();
            keyWord.ID = i;
            keyWord.Title = splitRow[0];
            keyWord.Description = splitRow[1];
            keyWord.Topic = int.Parse(splitRow[2]);
            GetKeyOperatorsForTest().Add(keyWord);
        }
    }

    private static void ImportKeyWordsForTest()
    {
        GetKeyWordsForTest().Clear();
        var dataset = Resources.Load<TextAsset>("keywords_for_test");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            var splitRow = splitDataset[i].Split(new char[] { ';' });
            KeyWord keyWord = new KeyWord();
            keyWord.ID = i;
            keyWord.Title = splitRow[0];
            keyWord.Description = splitRow[1];
            keyWord.Topic = int.Parse(splitRow[2]);
            GetKeyWordsForTest().Add(keyWord);
        }
    }

    private static void ImportKeyWordsForAlgo()
    {
        GetKeyWordsForAlgo().Clear();
        var dataset = Resources.Load<TextAsset>("keywords_list");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            GetKeyWordsForAlgo().Add(str);
        }
    }

    private static void ImportVarNames()
    {
        GetVarNames().Clear();
        var dataset = Resources.Load<TextAsset>("var_names_list");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            GetVarNames().Add(str);
        }
    }

    #endregion

    #region Algorithms

    /// <summary>
    /// Return word (int), which pass as params
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo0(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int intValue = rand.Next(1, 30);

        string rightAnswer = intValue.ToString();
        string codeAnswers = "print (\"" + rightAnswer + "\")";
        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongIntFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 1;
        int score = 10;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Print word (int), which pass as params
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo1(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int intValue = rand.Next(1, 30);

        string rightAnswer = intValue.ToString();
        string varName = GetRandomFromVarNames();

        string codeAnswers0 = varName + " = " + rightAnswer;
        string codeAnswers1 = "print (" + varName + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongIntFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 1;
        int score = 15;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Return word, which pass as params
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo2(int topic, int level, int step)
    {
        string rightAnswer = GetRandomFromVarNames();
        string codeAnswers = "print (\"" + rightAnswer + "\")";
        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongStringFromKeyWords(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 1;
        int score = 10;
        
        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Return word (with Random var name), which pass as params
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo3(int topic, int level, int step)
    {
        string rightAnswer = GetRandomFromVarNames();
        List<string> varNames = new List<string>();
        varNames.Add(rightAnswer);
        string varName = GetRandomFromVarNames(varNames);

        string codeAnswers0 = varName + " = " + rightAnswer;
        string codeAnswers1 = "print (" + varName + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongStringFromKeyWords(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 1;
        int score = 15;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Return word (float), which pass as params
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo4(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        float floatValue = (rand.Next(10, 100) * 10) / 1000f;

        string rightAnswer = floatValue.ToString();
        string codeAnswers = "print (\"" + rightAnswer + "\")";
        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongFloatFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 1;
        int score = 10;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// print word (floatValue), which pass as params
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo5(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        float floatValue = (rand.Next(10, 100) * 10) / 1000f;

        string rightAnswer = floatValue.ToString();
        string varName = GetRandomFromVarNames();

        string codeAnswers0 = varName + " = " + rightAnswer;
        string codeAnswers1 = "print (" + varName + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongFloatFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 1;
        int score = 15;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Sum of a + b, print (sum)
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo6(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(1, 20);
        int b = rand.Next(1, 20);
        int c = a + b;
        string rightAnswer = c.ToString();

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);

        string codeAnswers0 = varName1 + " = " + a;
        string codeAnswers1 = varName2 + " = " + b;
        string codeAnswers2 = varName3 + " = " + varName1 + " + " + varName2;
        string codeAnswers3 = "print (" + varName3 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongIntFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 40;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Sum of a - b, print (sum)
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo7(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(10, 30);
        int b = rand.Next(1, 9);
        int c = a - b;
        string rightAnswer = c.ToString();

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);

        string codeAnswers0 = varName1 + " = " + a;
        string codeAnswers1 = varName2 + " = " + b;
        string codeAnswers2 = varName3 + " = " + varName1 + " - " + varName2;
        string codeAnswers3 = "print (" + varName3 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongIntFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 40;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Total of a * b, print (Total)
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo8(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(1, 10);
        int b = rand.Next(1, 10);
        int c = a * b;
        string rightAnswer = c.ToString();

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);

        string codeAnswers0 = varName1 + " = " + a;
        string codeAnswers1 = varName2 + " = " + b;
        string codeAnswers2 = varName3 + " = " + varName1 + " * " + varName2;
        string codeAnswers3 = "print (" + varName3 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongIntFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 40;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Total of a / b, print (Total)
    /// </summary>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo9(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(1, 10);
        int b = rand.Next(1, 10);
        int c_0 = a * b;
        int c = a;
        a = c_0;
        string rightAnswer = c.ToString();

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);

        string codeAnswers0 = varName1 + " = " + a;
        string codeAnswers1 = varName2 + " = " + b;
        string codeAnswers2 = varName3 + " = " + varName1 + " / " + varName2;
        string codeAnswers3 = "print (" + varName3 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetWrongIntFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 40;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    #endregion

    #region Test

    public static Question Test_0_KeyWords(int topic, int level, int step)
    {
        List<KeyWord> kwList = GetKeyWordsListForTopic(topic);
        System.Random rand = new System.Random();
        KeyWord keyWordRight = kwList[rand.Next(0, kwList.Count)];

        List<KeyWord> keyWordsForTest = new List<KeyWord>();
        keyWordsForTest.Add(keyWordRight);
        KeyWord wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetRandomFromKeyWordsForTest(keyWordsForTest);
            keyWordsForTest.Add(wrongAnswer);
        }
        List<string> answersWords = new List<string>();
        foreach (var item in keyWordsForTest)
            answersWords.Add(item.Title);


        int difficulty = 1;
        int score = 10;

        return FillNewQuestionForTest(keyWordRight.Description, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Test_1_KeyOperators(int topic, int level, int step)
    {
        List<KeyWord> kwList = GetKeyOperatorsListForTopic(topic);
        System.Random rand = new System.Random();
        KeyWord keyOperatorRight = kwList[rand.Next(0, kwList.Count)];

        List<KeyWord> keyOperatorsForTest = new List<KeyWord>();
        keyOperatorsForTest.Add(keyOperatorRight);
        KeyWord wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetRandomFromKeyOperatorsForTest(keyOperatorsForTest);
            keyOperatorsForTest.Add(wrongAnswer);
        }
        List<string> answersWords = new List<string>();
        foreach (var item in keyOperatorsForTest)
            answersWords.Add(item.Title);


        int difficulty = 1;
        int score = 10;

        return FillNewQuestionForTest(keyOperatorRight.Description, answersWords, level, topic, step, difficulty, score);
    }

    #endregion

    #region methods

    private static Question FillNewQuestionForShelfTest(List<string> codeWords, List<string> answersWords, int topic, int level, int step, int difficulty, int score)
    {
        string title = "Какой результат будет после выполнения следующего кода:";
        return FillNewQuestion(QuestionType.ShelfTest, title, codeWords, answersWords, level, topic, step, difficulty, score);
    }

    private static Question FillNewQuestionForTest(string title, List<string> answersWords, int topic, int level, int step, int difficulty, int score)
    {
        List<string> codeWords = new List<string>();
        return FillNewQuestion(QuestionType.Test, title, codeWords, answersWords, level, topic, step, difficulty, score);
    }

    private static Question FillNewQuestion(QuestionType questionType, string title, List<string> codeWords, List<string> answersWords, int topic, int level, int step, int difficulty, int score)
    {
        Question question = new QuestionText();
        question.Difficulty = difficulty;
        question.Score = score;
        question.Level = level;
        question.Topic = topic;
        question.Step = step;
        question.IsSingleRightAnswer = true;
        question.CountShelves = codeWords.Count;
        question.QuestionType = questionType;
        question.Title = title;

        List<Answer> answers = new List<Answer>();
        int row = 0;
        foreach (var strShelf in codeWords)
        {
            Answer answer = new Answer();
            answer.Title = strShelf;
            answer.IsRight = true;
            answer.Score = 0;
            answer.IsPositionCellDependent = true;
            answer.IsPositionRowDependent = true;
            answer.PositionRowIndex = row;
            answer.PositionCellIndex = 0;
            answer.IsOpenOnStart = true;
            answers.Add(answer);
            row++;
        }
        int countAnswer = 0;
        foreach (var strAnswer in answersWords)
        {
            Answer answer = new Answer();
            answer.Title = strAnswer;
            answer.IsRight = countAnswer == 0;
            answer.Score = 0;
            answer.IsPositionCellDependent = false;
            answer.IsPositionRowDependent = false;
            answer.PositionRowIndex = row;
            answer.PositionCellIndex = 0;
            answer.IsOpenOnStart = false;
            answers.Add(answer);
            countAnswer++;
            row++;
        }
        answers.Shuffle();
        question.SetAnswerList(answers);

        return question;
    }

    private static string GetWrongStringFromKeyWords(List<string> stopWords)
    {
        bool containsWord = true;
        string strWord = GetKeyWordsForAlgo()[0];
        int maxRound = 100;
        System.Random rand = new System.Random();
        while (containsWord)
        {
            strWord = _keyWordsForAlgo[rand.Next(0, _keyWordsForAlgo.Count)];
            containsWord = stopWords.Contains(strWord);
            maxRound--;
            if (maxRound < 0)
                break;
        }
        return strWord;
    }

    private static string GetWrongIntFromRandom(List<string> stopWords)
    {
        bool containsWord = true;
        string strWord = "0";
        int maxRound = 100;
        System.Random rand = new System.Random();
        while (containsWord)
        {
            strWord = rand.Next(0, 100).ToString();
            containsWord = stopWords.Contains(strWord);
            maxRound--;
            if (maxRound < 0)
                break;
        }
        return strWord;
    }

    private static string GetWrongFloatFromRandom(List<string> stopWords)
    {
        bool containsWord = true;
        string strWord = "0.0";
        int maxRound = 100;
        System.Random rand = new System.Random();
        while (containsWord)
        {
            strWord = ((rand.Next(0, 10000))/100f).ToString();
            containsWord = stopWords.Contains(strWord);
            maxRound--;
            if (maxRound < 0)
                break;
        }
        return strWord;
    }

    private static string GetRandomFromVarNames()
    {
        System.Random rand = new System.Random();
        return GetVarNames()[rand.Next(0, GetVarNames().Count)];
    }

    private static string GetRandomFromVarNames(List<string> stopWords)
    {
        string strWord = "";

        bool containsWord = true;
        int maxRound = 100;
        while (containsWord)
        {
            strWord = GetRandomFromVarNames();
            containsWord = stopWords.Contains(strWord);
            maxRound--;
            if (maxRound < 0)
                break;
        }
        return strWord;
    }

    private static KeyWord GetRandomFromKeyWordsForTest()
    {
        System.Random rand = new System.Random();
        return GetKeyWordsForTest()[rand.Next(0, GetKeyWordsForTest().Count)];
    }

    private static KeyWord GetRandomFromKeyWordsForTest(List<KeyWord> stopWords)
    {
        KeyWord keyWord = null;

        bool containsWord = true;
        int maxRound = 100;
        while (containsWord)
        {
            keyWord = GetRandomFromKeyWordsForTest();
            containsWord = stopWords.Contains(keyWord);
            maxRound--;
            if (maxRound < 0)
                break;
        }
        return keyWord;
    }

    private static List<KeyWord> GetKeyWordsListForTopic(int topic)
    {
        List<KeyWord> list = new List<KeyWord>();
        List<KeyWord> listFull = GetKeyWordsForTest();
        foreach (var item in listFull)
        {
            if (item.Topic <= topic)
            {
                list.Add(item);
            }
        }
        return list;
    }

    private static KeyWord GetRandomFromKeyOperatorsForTest()
    {
        System.Random rand = new System.Random();
        return GetKeyOperatorsForTest()[rand.Next(0, GetKeyOperatorsForTest().Count)];
    }

    private static KeyWord GetRandomFromKeyOperatorsForTest(List<KeyWord> stopWords)
    {
        KeyWord keyWord = null;

        bool containsWord = true;
        int maxRound = 100;
        while (containsWord)
        {
            keyWord = GetRandomFromKeyOperatorsForTest();
            containsWord = stopWords.Contains(keyWord);
            maxRound--;
            if (maxRound < 0)
                break;
        }
        return keyWord;
    }

    private static List<KeyWord> GetKeyOperatorsListForTopic(int topic)
    {
        List<KeyWord> list = new List<KeyWord>();
        List<KeyWord> listFull = GetKeyOperatorsForTest();
        foreach (var item in listFull)
        {
            if (item.Topic <= topic)
            {
                list.Add(item);
            }
        }
        return list;
    }

    private static List<AlgoInfo> GetAlgoInfoListForTopicAndLevel(int topic, int level)
    {
        List<AlgoInfo> list = new List<AlgoInfo>();
        List<AlgoInfo> listFull = GetAlgoInfoList();
        
        foreach (var item in listFull)
        {
            if (item.StartTopic <= topic && item.FinishTopic >= topic)
            {
                if(
                    ((item.StartTopic == topic && item.StartLevel <= level) || item.StartTopic != topic)
                    &&
                    ((item.FinishTopic == topic && item.FinishLevel >= level) || item.FinishTopic != topic)
                    )
                    list.Add(item);
            }
        }
        return list;
    }

    #endregion

    public static Question GetQuestionFromAlgo(int topic, int level, int step)
    {
        List<AlgoInfo> listFull = GetAlgoInfoListForTopicAndLevel(topic, level);
        
        System.Random random = new System.Random();
        int index = random.Next(0, listFull.Count);
        AlgoInfo algoInfo = listFull[index];
        //if (topic <= 1)
        //{
        string methodName = algoInfo.Title;
        string[] parameters = { level.ToString(), level.ToString(), level.ToString() };
        Type type = typeof(AlgorithmTestContriller);
        MethodInfo info = type.GetMethod(methodName);
        System.Object obj = info.Invoke(null, new object[] { topic, level, step });
        return (Question)obj;

        //}
    }
}

public class KeyWord
{
    public int ID = 0;
    public string Title = "";
    public string Description = "";
    public int Topic = 0;
}

public class AlgoInfo
{
    public string Title = "";
    public int StartTopic = 0;
    public int StartLevel = 0;
    public int FinishTopic = 0;
    public int FinishLevel = 0;
    public int QuestionType = 0;
}
