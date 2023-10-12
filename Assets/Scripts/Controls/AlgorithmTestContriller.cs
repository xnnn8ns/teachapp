using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmTestContriller : MonoBehaviour
{
    private static List<string> keyWords;
    private static List<string> varNames;

    private static List<string> GetKeyWords()
    {
        if (keyWords == null)
        {
            keyWords = new List<string>();
            ImportKeyWords();
        }
        return keyWords;
    }

    private static List<string> GetVarNames()
    {
        if (varNames == null)
        {
            varNames = new List<string>();
            ImportVarNames();
        }
        return varNames;
    }

    private static void ImportKeyWords()
    {
        GetKeyWords().Clear();
        var dataset = Resources.Load<TextAsset>("keywords_list");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            GetKeyWords().Add(str);
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

    /// <summary>
    /// Return word (int), which pass as params
    /// </summary>
    /// <param name="intValue"></param>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo0(int intValue, int level, int topic, int step)
    {
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

        return FillValuesForNewQuestion(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo1(int intValue, int level, int topic, int step)
    {
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

        return FillValuesForNewQuestion(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Return word, which pass as params
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo2(string strValue, int level, int topic, int step)
    {
        string rightAnswer = strValue;
        string codeAnswers = "print (\"" + strValue + "\")";
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
        
        return FillValuesForNewQuestion(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Return word (with Random var name), which pass as params
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo3(string strValue, int level, int topic, int step)
    {
        string rightAnswer = strValue;
        string varName = GetRandomFromVarNames();

        string codeAnswers0 = varName + " = " + strValue;
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

        return FillValuesForNewQuestion(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    /// <summary>
    /// Return word (int), which pass as params
    /// </summary>
    /// <param name="floatValue"></param>
    /// <param name="level"></param>
    /// <param name="topic"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    public static Question Algo4(float floatValue, int level, int topic, int step)
    {
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

        return FillValuesForNewQuestion(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    public static Question Algo5(float floatValue, int level, int topic, int step)
    {
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

        return FillValuesForNewQuestion(codeWords, answersWords, level, topic, step, difficulty, score);
    }

    private static Question FillValuesForNewQuestion(List<string> codeWords, List<string> answersWords, int level, int topic, int step, int difficulty, int score)
    {
        Question question = new QuestionText();
        question.Difficulty = difficulty;
        question.Score = score;
        question.Level = level;
        question.Topic = topic;
        question.Step = step;
        question.IsSingleRightAnswer = true;
        question.CountShelves = codeWords.Count;
        question.QuestionType = QuestionType.ShelfTest;
        question.Title = "Какой результат будет после выполнения следующего кода:";

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
        question.SetAnswerList(answers);

        return question;
    }

    private static string GetWrongStringFromKeyWords(List<string> stopWords)
    {
        bool containsWord = true;
        string strWord = GetKeyWords()[0];
        int maxRound = 100;
        System.Random rand = new System.Random();
        while (containsWord)
        {
            strWord = keyWords[rand.Next(0, keyWords.Count)];
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
}
