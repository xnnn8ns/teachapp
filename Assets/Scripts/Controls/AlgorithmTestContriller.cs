using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmTestContriller : MonoBehaviour
{
    private static List<string> instance;

    private AlgorithmTestContriller()
    {

    }

    public static List<string> getInstance()
    {
        if (instance == null)
        {
            instance = new List<string>();
            ImportKeyWords();
        }
        return instance;
    }

    private static void ImportKeyWords()
    {
        getInstance().Clear();
        var dataset = Resources.Load<TextAsset>("keywords_list");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string str = splitDataset[i];
            Debug.Log(str);
            getInstance().Add(str);
        }
    }

    public static Question Algo1(string StrValue, int level, int topic, int step)
    {
        string rightAnswer = Algorithm.GetSimplePrintStr(StrValue);
        string codeAnswers = "print (\"" + StrValue + "\")";
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
        Question question = new QuestionText();
        question.Title = "Какой результат будет после выполнения следующего кода:";
        question.Difficulty = 1;
        question.CountShelves = codeWords.Count;
        question.QuestionType = QuestionType.ShelfTest;
        question.Score = 10;
        question.Level = level;
        question.Topic = topic;
        question.Step = step;
        question.IsSingleRightAnswer = true;
        question.CountShelves = 1;
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
        string strWord = getInstance()[0];
        int maxRound = 100;
        System.Random rand = new System.Random();
        while (containsWord)
        {
            strWord = instance[rand.Next(0, instance.Count)];
            containsWord = stopWords.Contains(strWord);
            maxRound--;
            if (maxRound < 0)
                break;
        }
        return strWord;
    }
}
