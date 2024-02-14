using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class AlgorithmTestContriller : MonoBehaviour
{
    private static string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static List<string> _keyWordsForAlgo;
    private static List<KeyWord> _keyWordsForTestList = null;
    private static List<KeyWord> _keyOperatorsForTestList = null;
    private static List<KeyWord> _keyBuildInsForTestList = null;
    private static List<AlgoInfo> _algoInfoList = null;
    private static List<string> _varNames;
    private static List<string> _varStrings;
    private static List<string> _keyComparison;
    private static List<string> _keyLogic;
    private static List<string> _keyOperandFunc;
    private static List<string> _keyBuildInFunc;

    #region KeyMethods

    public static void AlgoUpdateLang()
    {
        _keyWordsForTestList = null;
        _keyOperatorsForTestList = null;
        _keyBuildInsForTestList = null;
        _algoInfoList = null;
    }

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

    private static List<KeyWord> GetKeyBuildInsForTest()
    {
        if (_keyBuildInsForTestList == null)
        {
            _keyBuildInsForTestList = new List<KeyWord>();
            ImportKeyBuildInsForTest();
        }
        return _keyBuildInsForTestList;
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

    private static List<string> GetVarStrings()
    {
        if (_varStrings == null)
        {
            _varStrings = new List<string>();
            ImportVarStrings();
        }
        return _varStrings;
    }

    private static List<string> GetKeyComparison()
    {
        if (_keyComparison == null)
        {
            _keyComparison = new List<string>();
            ImportKeyComparison();
        }
        return _keyComparison;
    }

    private static List<string> GetKeyLogic()
    {
        if (_keyLogic == null)
        {
            _keyLogic = new List<string>();
            ImportKeyLogic();
        }
        return _keyLogic;
    }

    private static List<string> GetKeyOperandFunc()
    {
        if (_keyOperandFunc == null)
        {
            _keyOperandFunc = new List<string>();
            ImportKeyOperandFunc();
        }
        return _keyOperandFunc;
    }

    private static List<string> GetKeyBuildInFunc()
    {
        if (_keyBuildInFunc == null)
        {
            _keyBuildInFunc = new List<string>();
            ImportKeyBuildInFunc();
        }
        return _keyBuildInFunc;
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
            //Debug.Log(splitRow[0]);
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

        string lang = "en";
        if (LangAsset.CurrentLangLocation == LangLocation.En)
            lang = "en";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ru)
            lang = "ru";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ge)
            lang = "ge";
        else if (LangAsset.CurrentLangLocation == LangLocation.It)
            lang = "it";

        var dataset = Resources.Load<TextAsset>("keys/keyoperators_for_test/keyoperators_for_test_" + lang);
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

    private static void ImportKeyBuildInsForTest()
    {
        GetKeyBuildInsForTest().Clear();

        string lang = "en";
        if (LangAsset.CurrentLangLocation == LangLocation.En)
            lang = "en";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ru)
            lang = "ru";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ge)
            lang = "ge";
        else if (LangAsset.CurrentLangLocation == LangLocation.It)
            lang = "it";

        var dataset = Resources.Load<TextAsset>("keys/keywords_build_in_for_test/keywords_build_in_for_test_" + lang);
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            var splitRow = splitDataset[i].Split(new char[] { ';' });
            KeyWord keyWord = new KeyWord();
            keyWord.ID = i;
            keyWord.Title = splitRow[0];
            keyWord.Description = splitRow[1];
            keyWord.Topic = int.Parse(splitRow[2]);
            GetKeyBuildInsForTest().Add(keyWord);
        }
    }

    private static void ImportKeyWordsForTest()
    {
        GetKeyWordsForTest().Clear();

        string lang = "en";
        if (LangAsset.CurrentLangLocation == LangLocation.En)
            lang = "en";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ru)
            lang = "ru";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ge)
            lang = "ge";
        else if (LangAsset.CurrentLangLocation == LangLocation.It)
            lang = "it";
        //Debug.Log("Keys/keywords_for_test/keywords_for_test_" + lang);
        var dataset = Resources.Load<TextAsset>("Keys/keywords_for_test/keywords_for_test_" + lang);
        
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            var splitRow = splitDataset[i].Split(new char[] { ';' });
            //Debug.Log(splitRow[0]);
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
        var dataset = Resources.Load<TextAsset>("keys/keywords_list");
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
        var dataset = Resources.Load<TextAsset>("keys/var_names_list");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            GetVarNames().Add(str);
        }
    }

    private static void ImportVarStrings()
    {
        GetVarStrings().Clear();
        var dataset = Resources.Load<TextAsset>("keys/var_string_list");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            GetVarStrings().Add(str);
        }
    }

    private static void ImportKeyComparison()
    {
        GetKeyComparison().Clear();
        var dataset = Resources.Load<TextAsset>("keys/keywords_comparison");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            GetKeyComparison().Add(str);
        }
    }

    private static void ImportKeyLogic()
    {
        GetKeyLogic().Clear();
        var dataset = Resources.Load<TextAsset>("keys/keywords_logic");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            GetKeyLogic().Add(str);
        }
    }

    private static void ImportKeyOperandFunc()
    {
        GetKeyOperandFunc().Clear();
        var dataset = Resources.Load<TextAsset>("keys/keyoperators_for_func");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            //Debug.Log(str);
            GetKeyOperandFunc().Add(str);
        }
    }

    private static void ImportKeyBuildInFunc()
    {
        GetKeyBuildInFunc().Clear();

        string lang = "en";
        if (LangAsset.CurrentLangLocation == LangLocation.En)
            lang = "en";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ru)
            lang = "ru";
        else if (LangAsset.CurrentLangLocation == LangLocation.Ge)
            lang = "ge";
        else if (LangAsset.CurrentLangLocation == LangLocation.It)
            lang = "it";

        var dataset = Resources.Load<TextAsset>("keywords_build_in_for_test/keywords_build_in_for_test_" + lang);
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            //Debug.Log(str);
            GetKeyBuildInFunc().Add(str);
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

    public static Question Algo0_1(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int intValue = rand.Next(1, 30);

        string rightAnswerOriginal = intValue.ToString();
        string codeAnswers = "str (\"" + rightAnswerOriginal + "\")";
        string rightAnswer = "\"" + rightAnswerOriginal + "\"";
        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer = "\'" + rightAnswerOriginal + "\'";
        answersWords.Add(wrongAnswer);
        wrongAnswer = rightAnswerOriginal;
        answersWords.Add(wrongAnswer);
        wrongAnswer = rightAnswerOriginal + ".0";
        answersWords.Add(wrongAnswer);

        int difficulty = 1;
        int score = 10;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo0_2(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int intValue1 = rand.Next(1, 10);
        int intValue2 = rand.Next(1, 10);

        string rightAnswerOriginal = intValue1.ToString() + intValue2.ToString();
        string codeAnswers = "str (\"" + intValue1 + "\") + str(\"" + intValue2 + "\")";
        string rightAnswer = "\"" + rightAnswerOriginal + "\"";
        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer = "\'" + rightAnswerOriginal + "\'";
        answersWords.Add(wrongAnswer);
        wrongAnswer = (intValue1 + intValue2).ToString();
        answersWords.Add(wrongAnswer);
        wrongAnswer = "\"" + wrongAnswer + "\"";
        answersWords.Add(wrongAnswer);

        int difficulty = 1;
        int score = 15;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo0_3(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int intValue1 = rand.Next(1, 10);
        int intValue2 = rand.Next(1, 10);

        string rightAnswerOriginal = intValue1.ToString() + intValue2.ToString();
        string codeAnswers = "str (" + intValue1 + ") + str(" + intValue2 + ")";
        string rightAnswer = "\"" + rightAnswerOriginal + "\"";
        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer = "\'" + rightAnswerOriginal + "\'";
        answersWords.Add(wrongAnswer);
        wrongAnswer = (intValue1 + intValue2).ToString();
        answersWords.Add(wrongAnswer);
        wrongAnswer = "\"" + wrongAnswer + "\"";
        answersWords.Add(wrongAnswer);

        int difficulty = 1;
        int score = 15;

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

    public static Question Algo10(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int intValue1 = rand.Next(1, 10);
        int intValue2 = rand.Next(1, 10);

        string rightAnswer;
        string wrongAnswer;
        string comparison = GetRandomKeyComparison();
        bool rusult;
        switch (comparison)
        {
            case "==":
                rusult = intValue1 == intValue2;
                break;
            case "!=":
                rusult = intValue1 != intValue2;
                break;
            case ">":
                rusult = intValue1 > intValue2;
                break;
            case "<":
                rusult = intValue1 < intValue2;
                break;
            case ">=":
                rusult = intValue1 >= intValue2;
                break;
            case "<=":
                rusult = intValue1 <= intValue2;
                break;
            default:
                rusult = false;
                break;
        }
        if (rusult)
        {
            rightAnswer = "True";
            wrongAnswer = "False";
        }
        else
        {
            rightAnswer = "False";
            wrongAnswer = "True";
        }


        string codeAnswers0 = "print (" + intValue1.ToString() + comparison + intValue2.ToString() + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        answersWords.Add(wrongAnswer);
        for (int i = 0; i < 2; i++)
        {
            wrongAnswer = GetRandomFromVarNames(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 1;
        int score = 30;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo11(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(1, 10);
        int b = rand.Next(1, 10);
        string rightAnswer;
        string wrongAnswer;

        string comparison = GetRandomKeyComparison();
        bool rusult;
        switch (comparison)
        {
            case "==":
                rusult = a == b;
                break;
            case "!=":
                rusult = a != b;
                break;
            case ">":
                rusult = a > b;
                break;
            case "<":
                rusult = a < b;
                break;
            case ">=":
                rusult = a >= b;
                break;
            case "<=":
                rusult = a <= b;
                break;
            default:
                rusult = false;
                break;
        }

        if (rusult)
        {
            rightAnswer = "True";
            wrongAnswer = "False";
        }
        else
        {
            rightAnswer = "False";
            wrongAnswer = "True";
        }

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);

        string codeAnswers0 = varName1 + " = " + a;
        string codeAnswers1 = varName2 + " = " + b;
        string codeAnswers2 = varName3 + " = " + varName1 + " " + comparison + " " + varName2;
        string codeAnswers3 = "print (" + varName3 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        answersWords.Add(wrongAnswer);
        for (int i = 0; i < 2; i++)
        {
            wrongAnswer = GetRandomFromVarNames(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 40;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo12(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int intValue1 = rand.Next(1, 6);
        int intValue2 = rand.Next(2, 8);

        int intValue3 = rand.Next(20, 25);
        int intValue4 = rand.Next(22, 28);

        string rightAnswer;
        string wrongAnswer;
        string comparison1 = GetRandomKeyComparison();
        string comparison2 = GetRandomKeyComparison();
        bool rusult1;
        switch (comparison1)
        {
            case "==":
                rusult1 = intValue1 == intValue2;
                break;
            case "!=":
                rusult1 = intValue1 != intValue2;
                break;
            case ">":
                rusult1 = intValue1 > intValue2;
                break;
            case "<":
                rusult1 = intValue1 < intValue2;
                break;
            case ">=":
                rusult1 = intValue1 >= intValue2;
                break;
            case "<=":
                rusult1 = intValue1 <= intValue2;
                break;
            default:
                rusult1 = false;
                break;
        }
        bool rusult2;
        switch (comparison2)
        {
            case "==":
                rusult2 = intValue3 == intValue4;
                break;
            case "!=":
                rusult2 = intValue3 != intValue4;
                break;
            case ">":
                rusult2 = intValue3 > intValue4;
                break;
            case "<":
                rusult2 = intValue3 < intValue4;
                break;
            case ">=":
                rusult2 = intValue3 >= intValue4;
                break;
            case "<=":
                rusult2 = intValue3 <= intValue4;
                break;
            default:
                rusult2 = false;
                break;
        }
        string logic = GetRandomKeyLogic();
        bool rusult3;
        switch (logic)
        {
            case "and":
                rusult3 = rusult1 && rusult2;
                break;
            case "or":
                rusult3 = rusult1 || rusult2;
                break;
            default:
                rusult3 = false;
                break;
        }
        if (rusult3)
        {
            rightAnswer = "True";
            wrongAnswer = "False";
        }
        else
        {
            rightAnswer = "False";
            wrongAnswer = "True";
        }


        string codeAnswers0 = "print ( " + intValue1.ToString() + comparison1 + intValue2.ToString()
            + " " + logic + " "
            + intValue3.ToString() + comparison2 + intValue4.ToString() + " )";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        answersWords.Add(wrongAnswer);
        for (int i = 0; i < 2; i++)
        {
            wrongAnswer = GetRandomFromVarNames(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 50;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo13(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(1, 6);
        int b = rand.Next(2, 8);
        int x = rand.Next(12, 16);
        int y = rand.Next(14, 18);
        string rightAnswer;
        string wrongAnswer;

        string comparison1 = GetRandomKeyComparison();
        bool rusult1;
        switch (comparison1)
        {
            case "==":
                rusult1 = a == b;
                break;
            case "!=":
                rusult1 = a != b;
                break;
            case ">":
                rusult1 = a > b;
                break;
            case "<":
                rusult1 = a < b;
                break;
            case ">=":
                rusult1 = a >= b;
                break;
            case "<=":
                rusult1 = a <= b;
                break;
            default:
                rusult1 = false;
                break;
        }

        string comparison2 = GetRandomKeyComparison();
        bool rusult2;
        switch (comparison2)
        {
            case "==":
                rusult2 = x == y;
                break;
            case "!=":
                rusult2 = x != y;
                break;
            case ">":
                rusult2 = x > y;
                break;
            case "<":
                rusult2 = x < y;
                break;
            case ">=":
                rusult2 = x >= y;
                break;
            case "<=":
                rusult2 = x <= y;
                break;
            default:
                rusult2 = false;
                break;
        }

        string logic = GetRandomKeyLogic();
        bool rusult3;
        switch (logic)
        {
            case "and":
                rusult3 = rusult1 && rusult2;
                break;
            case "or":
                rusult3 = rusult1 || rusult2;
                break;
            default:
                rusult3 = false;
                break;
        }
        if (rusult3)
        {
            rightAnswer = "True";
            wrongAnswer = "False";
        }
        else
        {
            rightAnswer = "False";
            wrongAnswer = "True";
        }

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);
        varWords.Add(varName3);
        string varName4 = GetRandomFromVarNames(varWords);
        varWords.Add(varName4);

        string codeAnswers0 = varName1 + " = " + a;
        string codeAnswers1 = varName2 + " = " + b;
        string codeAnswers2 = varName3 + " = " + x;
        string codeAnswers3 = varName4 + " = " + y;
        string codeAnswers4 = "print ( " + varName1 + comparison1 + varName2
            + " " + logic + " "
            + varName3 + comparison2 + varName4 + " )";


        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);
        codeWords.Add(codeAnswers4);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        answersWords.Add(wrongAnswer);
        for (int i = 0; i < 2; i++)
        {
            wrongAnswer = GetRandomFromVarNames(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 40;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo14(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        string varString1 = GetRandomFromVarStrings();
        int a = rand.Next(0, varString1.Length - 3);
        int b = rand.Next(a, varString1.Length - 1);
        string varString2 = varString1.Substring(a,b-a);
        string rightAnswer = varString2;

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);

        string codeAnswers0 = varName1 + " = \"" + varString1 + "\"";
        string codeAnswers1 = varName2 + " = [" + a.ToString() + ":" + b.ToString() + "]";
        string codeAnswers2 = "print (" + varName2 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetRandomSubstring(varString1, a, b, answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 50;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo15(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        string varString1 = GetRandomFromVarStrings();
        int a = rand.Next(0, varString1.Length - 2);
        string varString2 = varString1.Substring(a);
        string rightAnswer = varString2;

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);

        string codeAnswers0 = varName1 + " = \"" + varString1 + "\"";
        string codeAnswers1 = varName2 + " = [" + a.ToString() + ":]";
        string codeAnswers2 = "print (" + varName2 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetRandomSubstring(varString1, a, varString1.Length - 1, answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 50;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo16(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        string varString1 = GetRandomFromVarStrings();
        int a = rand.Next(1, varString1.Length - 1);
        string varString2 = varString1.Substring(0,a);
        string rightAnswer = varString2;

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);

        string codeAnswers0 = varName1 + " = \"" + varString1 + "\"";
        string codeAnswers1 = varName2 + " = [:" + a.ToString() + "]";
        string codeAnswers2 = "print (" + varName2 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetRandomSubstring(varString1, a, varString1.Length - 1, answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 50;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo17(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        string varString1 = GetRandomFromVarStrings();
        int a = rand.Next(1, varString1.Length - 1);
        string varString2 = varString1[a].ToString();
        string rightAnswer = varString2;

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);

        string codeAnswers0 = varName1 + " = \"" + varString1 + "\"";
        string codeAnswers1 = varName2 + " = [" + a.ToString() + "]";
        string codeAnswers2 = "print (" + varName2 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        string wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetRandomSubstring(varString1, a, varString1.Length - 1, answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 50;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo18(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int intValue1 = rand.Next(1, 8);
        int intValue2 = rand.Next(1, 8);
        intValue1 += intValue2;

        string operand = GetRandomKeyOperandFunc();
        int result;
        string operandSec;
        Debug.Log(operand);
        if (operand.Contains("+"))
        {
            result = intValue1 + intValue2;
            operandSec = "+";
        }
        else if (operand.Contains("-"))
        {
            result = intValue1 - intValue2;
            operandSec = "-";
        }
        else if (operand.Contains("*"))
        {
            result = intValue1 * intValue2;
            operandSec = "*";
        }
        else if (operand.Contains("/"))
        {
            operandSec = "/";
            int res = intValue2 * rand.Next(2, 7);
            intValue1 = res;
            result = intValue1 / intValue2;
        }
        else
        {
            operandSec = "+";
            result = 0;
        }
        Debug.Log(result);
        string rightAnswer = result.ToString();

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);
        varWords.Add(varName3);

        

        string codeAnswers0 = "def sum(" + varName1 + ", " + varName2 + "):";
        string codeAnswers1 = "      return " + varName1 + " " + operandSec + " " + varName2;
        string codeAnswers2 = varName3 + " = " + " sum(" + intValue1.ToString() + ", " + intValue2.ToString() + ")";
        string codeAnswers3 = "print(" + varName3 + ")";
        Debug.Log(codeAnswers1);
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
        int score = 60;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo19(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(1, 100);
        int b = rand.Next(1, 100);
        string rightAnswer;
        string wrongAnswer;

        string comparison = GetRandomKeyComparison();
        bool rusult;
        switch (comparison)
        {
            case "==":
                rusult = a == b;
                break;
            case "!=":
                rusult = a != b;
                break;
            case ">":
                rusult = a > b;
                break;
            case "<":
                rusult = a < b;
                break;
            case ">=":
                rusult = a >= b;
                break;
            case "<=":
                rusult = a <= b;
                break;
            default:
                rusult = false;
                break;
        }

        if (rusult)
        {
            rightAnswer = a.ToString();
            wrongAnswer = b.ToString();
        }
        else
        {
            rightAnswer = b.ToString();
            wrongAnswer = a.ToString();
        }

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);

        string codeAnswers0 = varName1 + " = " + a;
        string codeAnswers1 = varName2 + " = " + b;
        string codeAnswers2 = "if (" + varName1 + " " + comparison + " " + varName2 + "):";
        string codeAnswers3 = "      print (" + varName1 + ")";
        string codeAnswers4 = "else:";
        string codeAnswers5 = "      print (" + varName2 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);
        codeWords.Add(codeAnswers4);
        codeWords.Add(codeAnswers5);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        answersWords.Add(wrongAnswer);
        for (int i = 0; i < 2; i++)
        {
            wrongAnswer = GetWrongIntFromRandom(answersWords);
            answersWords.Add(wrongAnswer);
        }
        int difficulty = 2;
        int score = 70;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo20(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(5, 12);
        int b = rand.Next(12, 20);
        int c = rand.Next(1, 5);
        string rightAnswer;
        string wrongAnswer;

        string comparison = GetRandomKeyComparison();
        bool rusult;
        switch (comparison)
        {
            case "==":
                rusult = a == b;
                break;
            case "!=":
                rusult = a != b;
                break;
            case ">":
                rusult = a > b;
                break;
            case "<":
                rusult = a < b;
                break;
            case ">=":
                rusult = a >= b;
                break;
            case "<=":
                rusult = a <= b;
                break;
            default:
                rusult = false;
                break;
        }
        string operandSec = "+";
        int result2;
        string operand = GetRandomKeyOperandFunc();

        if (operand.Contains("+"))
        {
            result2 = a + c;
            operandSec = "+";
        }
        else if (operand.Contains("-"))
        {
            result2 = a - c;
            operandSec = "-";
        }
        else if (operand.Contains("*"))
        {
            result2 = a * c;
            operandSec = "*";
        }
        else if (operand.Contains("/"))
        {
            operandSec = "/";
            result2 = a / c;
        }
        else
        {
            operandSec = "+";
            result2 = 0;
        }
        if (rusult)
        {
            rightAnswer = result2.ToString();
            wrongAnswer = a.ToString();
        }
        else
        {
            rightAnswer = a.ToString();
            wrongAnswer = result2.ToString();
        }

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);
        varWords.Add(varName3);

        string codeAnswers0 = varName1 + " = " + a;
        string codeAnswers1 = varName2 + " = " + b;
        string codeAnswers2 = "if (" + varName1 + " " + comparison + " " + varName2 + "):";
        string codeAnswers3 = "      " + varName1 + " = " + varName1 + " " + operandSec + " " + c.ToString();
        string codeAnswers4 = "print(" + varName1 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);
        codeWords.Add(codeAnswers4);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        answersWords.Add(wrongAnswer);
        for (int i = 0; i < 2; i++)
        {
            string str = GetWrongIntFromRandom(answersWords);
            answersWords.Add(str);
        }
        int difficulty = 3;
        int score = 110;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo21(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a_start = rand.Next(5, 11);
        int a = a_start;
        int b_start = rand.Next(1, 15);
        int b = b_start;
        int c = rand.Next(1, 3);
        int d = rand.Next(1, 3);
        string rightAnswer;

        while (a > 0)
        {
            b += c;
            a -= d;
        }

        rightAnswer = b.ToString();

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);

        string codeAnswers0 = varName1 + " = " + a_start;
        string codeAnswers1 = varName2 + " = " + b_start;
        string codeAnswers2 = "while (" + varName1 + " > 0):";
        string codeAnswers3 = "      " + varName2 + " += " + c.ToString();
        string codeAnswers4 = "      " + varName1 + " -= " + d.ToString();
        string codeAnswers5 = "print(" + varName2 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);
        codeWords.Add(codeAnswers4);
        codeWords.Add(codeAnswers5);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        for (int i = 0; i < 3; i++)
        {
            string str = GetWrongIntFromRandom(answersWords);
            answersWords.Add(str);
        }
        int difficulty = 3;
        int score = 120;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo22(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a = rand.Next(1, 4);
        int b = rand.Next(1, 4);
        int c = rand.Next(1, 5);
        int d = rand.Next(1, 5);

        int[] list = { a, b, c, d };

        int total = 0;

        string rightAnswer;

        foreach (var item in list)
        {
            total += item;
        }

        rightAnswer = total.ToString();

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);
        string varName3 = GetRandomFromVarNames(varWords);
        varWords.Add(varName3);

        string codeAnswers0 = varName1 + " = [" + a + "," + b + "," + c + "," + d + "]";
        string codeAnswers1 = varName2 + " = 0";
        string codeAnswers2 = "for " + varName3 + " in " + varName1 + ":";
        string codeAnswers3 = "      " + varName2 + " += " + varName3;
        string codeAnswers4 = "print(" + varName2 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);
        codeWords.Add(codeAnswers4);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        for (int i = 0; i < 3; i++)
        {
            string str = GetWrongIntFromRandom(answersWords);
            answersWords.Add(str);
        }
        int difficulty = 3;
        int score = 120;

        return FillNewQuestionForShelfTest(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo23(int topic, int level, int step)
    {
        System.Random rand = new System.Random();
        int a_start = rand.Next(5, 11);
        int a = a_start;
        int b_start = rand.Next(1, 15);
        int b = b_start;
        int c = rand.Next(1, 3);
        int d = rand.Next(1, 3);
        int e = rand.Next(3, 8);
        string rightAnswer;

        while (a > 0)
        {
            b += c;
            a -= d;
            if (b > e)
                break;
        }

        rightAnswer = b.ToString();

        List<string> varWords = new List<string>();

        string varName1 = GetRandomFromVarNames();
        varWords.Add(varName1);
        string varName2 = GetRandomFromVarNames(varWords);
        varWords.Add(varName2);

        string codeAnswers0 = varName1 + " = " + a_start;
        string codeAnswers1 = varName2 + " = " + b_start;
        string codeAnswers2 = "while (" + varName1 + " > 0):";
        string codeAnswers3 = "      " + varName2 + " += " + c.ToString();
        string codeAnswers4 = "      " + varName1 + " -= " + d.ToString();
        string codeAnswers5 = "if (" + varName2 + " > " + e + "):";
        string codeAnswers6 = "      break";
        string codeAnswers7 = "print(" + varName2 + ")";

        List<string> codeWords = new List<string>();
        codeWords.Add(codeAnswers0);
        codeWords.Add(codeAnswers1);
        codeWords.Add(codeAnswers2);
        codeWords.Add(codeAnswers3);
        codeWords.Add(codeAnswers4);
        codeWords.Add(codeAnswers5);
        codeWords.Add(codeAnswers6);
        codeWords.Add(codeAnswers7);

        List<string> answersWords = new List<string>();
        answersWords.Add(rightAnswer);
        for (int i = 0; i < 3; i++)
        {
            string str = GetWrongIntFromRandom(answersWords);
            answersWords.Add(str);
        }
        int difficulty = 3;
        int score = 120;

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

    public static Question Test_2_KeyBuildIns(int topic, int level, int step)
    {
        List<KeyWord> kwList = GetKeyBuildInsListForTopic(topic);
        System.Random rand = new System.Random();
        KeyWord keyBuildInRight = kwList[rand.Next(0, kwList.Count)];

        List<KeyWord> keyBuildInsForTest = new List<KeyWord>();
        keyBuildInsForTest.Add(keyBuildInRight);
        KeyWord wrongAnswer;
        for (int i = 0; i < 3; i++)
        {
            wrongAnswer = GetRandomFromKeyBuildInsForTest(keyBuildInsForTest);
            keyBuildInsForTest.Add(wrongAnswer);
        }
        List<string> answersWords = new List<string>();
        foreach (var item in keyBuildInsForTest)
            answersWords.Add(item.Title);


        int difficulty = 1;
        int score = 10;

        return FillNewQuestionForTest(keyBuildInRight.Description, answersWords, level, topic, step, difficulty, score);
    }

    #endregion

    #region methods

    private static Question FillNewQuestionForShelfTest(List<string> codeWords, List<string> answersWords, int topic, int level, int step, int difficulty, int score)
    {
        //string title = "Какой результат будет после выполнения следующего кода:";
        string title = LangAsset.GetValueByKey("WhatResultAfterCode");
        return FillNewQuestion(QuestionType.ShelfTest, title, codeWords, answersWords, level, topic, step, difficulty, score);
    }

    private static Question FillNewQuestionForTest(string title, List<string> answersWords, int topic, int level, int step, int difficulty, int score)
    {
        List<string> codeWords = new List<string>();
        return FillNewQuestion(QuestionType.Test, title, codeWords, answersWords, level, topic, step, difficulty, score);
    }

    private static Question FillNewQuestion(QuestionType questionType, string title, List<string> codeWords, List<string> answersWords, int topic, int level, int passCount, int difficulty, int score)
    {
        Question question = new QuestionText();
        question.Difficulty = difficulty;
        question.Score = score;
        question.Level = level;
        question.Topic = topic;
        question.Step = passCount;
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
        //if (_keyWordsForAlgo == null)
        //{
        //    _keyWordsForAlgo = new List<string>(); // Инициализируем _keyWordsForAlgo, если он равен null
        //}
        //if (_keyWordsForAlgo.Count == 0 || _keyWordsForAlgo.All(word => stopWords.Contains(word)))
        //{
        //    return ""; // Возвращаем пустую строку, если список _keyWordsForAlgo пуст или все его слова содержатся в списке stopWords
        //}

        bool containsWord = true;
        string strWord = _keyWordsForAlgo[0];
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

    private static string GetRandomFromVarStrings()
    {
        System.Random rand = new System.Random();
        return GetVarStrings()[rand.Next(0, GetVarStrings().Count)];
    }

    private static string GetRandomKeyComparison()
    {
        System.Random rand = new System.Random();
        return GetKeyComparison()[rand.Next(0, GetKeyComparison().Count)];
    }

    private static string GetRandomKeyLogic()
    {
        System.Random rand = new System.Random();
        return GetKeyLogic()[rand.Next(0, GetKeyLogic().Count)];
    }

    private static string GetRandomKeyOperandFunc()
    {
        System.Random rand = new System.Random();
        return GetKeyOperandFunc()[rand.Next(0, GetKeyOperandFunc().Count)];
    }

    private static string GetRandomKeyBuildInFunc()
    {
        System.Random rand = new System.Random();
        return GetKeyBuildInFunc()[rand.Next(0, GetKeyBuildInFunc().Count)];
    }

    private static string GetRandomSubstring(string baseString, int start, int finish, List<string> stopWords)
    {
        int newStart = start;
        int newFinish = finish;
        while (newStart > 0)
        {
            newStart--;
            newFinish--;
            if (!GetIsContainStrings(baseString.Substring(newStart, newFinish - newStart), stopWords))
                return baseString.Substring(newStart, newFinish - newStart);
        }
        newStart = start;
        newFinish = finish;
        while (newFinish < baseString.Length - 1)
        {
            newStart++;
            newFinish++;
            if (!GetIsContainStrings(baseString.Substring(newStart, newFinish - newStart), stopWords))
                return baseString.Substring(newStart, newFinish - newStart);
        }

        newStart = start;
        newFinish = finish;
        while (newFinish - newStart > 0)
        {
            newStart++;
            if (!GetIsContainStrings(baseString.Substring(newStart, newFinish - newStart), stopWords))
                return baseString.Substring(newStart, newFinish - newStart);
        }

        newStart = start;
        newFinish = finish;
        while (newFinish - newStart > 0)
        {
            newFinish--;
            if (!GetIsContainStrings(baseString.Substring(newStart, newFinish - newStart), stopWords))
                return baseString.Substring(newStart, newFinish - newStart);
        }

        newStart = start;
        newFinish = finish;
        while (newStart > 0 && newFinish < baseString.Length - 1)
        {
            newStart--;
            newFinish++;
            if (!GetIsContainStrings(baseString.Substring(newStart, newFinish - newStart), stopWords))
                return baseString.Substring(newStart, newFinish - newStart);
        }

        System.Random rand = new System.Random();

        return baseString + rand.Next(0, chars.Length); ;
    }

    private static bool GetIsContainStrings(string example, List<string> stopWords)
    {
        foreach (var item in stopWords)
        {
            if (example == item)
                return true;
        }
        return false;
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

    private static string GetRandomFromVarStrings(List<string> stopWords)
    {
        string strWord = "";

        bool containsWord = true;
        int maxRound = 100;
        while (containsWord)
        {
            strWord = GetRandomFromVarStrings();
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

    private static KeyWord GetRandomFromKeyBuildInsForTest()
    {
        System.Random rand = new System.Random();
        return GetKeyBuildInsForTest()[rand.Next(0, GetKeyBuildInsForTest().Count)];
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

    private static KeyWord GetRandomFromKeyBuildInsForTest(List<KeyWord> stopWords)
    {
        KeyWord keyWord = null;

        bool containsWord = true;
        int maxRound = 100;
        while (containsWord)
        {
            keyWord = GetRandomFromKeyBuildInsForTest();
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
                list.Add(item);
        }
        return list;
    }

    private static List<KeyWord> GetKeyBuildInsListForTopic(int topic)
    {
        List<KeyWord> list = new List<KeyWord>();
        List<KeyWord> listFull = GetKeyBuildInsForTest();
        foreach (var item in listFull)
        {
            if (item.Topic <= topic)
                list.Add(item);
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
                //if (item.FinishTopic == 10)
                //{
                //    Debug.Log(topic + "--" + level);
                //    Debug.Log(item.StartTopic + "-" + item.FinishTopic + "-" + item.StartLevel + "-" + item.FinishLevel);
                //}
                if (
                    ((item.StartTopic == topic && item.StartLevel <= level) || item.StartTopic != topic)
                    &&
                    ((item.FinishTopic == topic && item.FinishLevel >= level) || item.FinishTopic != topic)
                    )
                {
                    
                    list.Add(item);
                }
            }
        }
        return list;
    }

    #endregion

    public static Question GetQuestionFromAlgo(int topic, int level, int passCount)
    {
        //Debug.Log(topic + "-" + level);
        int levelID = Settings.GetLevelFromButtonOnMapID(level);
        //Debug.Log(topic + "-" + levelID);
        List<AlgoInfo> listFull = GetAlgoInfoListForTopicAndLevel(topic, levelID);
        
        System.Random random = new System.Random();
        int index = random.Next(0, listFull.Count);
        //Debug.Log(topic + "-" + listFull.Count);
        AlgoInfo algoInfo = listFull[index];
        string methodName = algoInfo.Title;
        Type type = typeof(AlgorithmTestContriller);
        MethodInfo info = type.GetMethod(methodName);
        System.Object obj = info.Invoke(null, new object[] { topic, level, passCount });
        return (Question)obj;
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
