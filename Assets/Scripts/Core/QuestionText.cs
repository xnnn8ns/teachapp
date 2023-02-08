using System;
using System.Collections.Generic;

public class QuestionText: Question
{
    public void FillAnswerList(List<string> answerStringList, List<bool> isAnswerRightList, List<int> answerScoreList)
    {
        Answer answer;
        List<Answer> answers = new List<Answer>();
        for (int i = 0; i < answerStringList.Count; i++)
        {
            answer = new Answer();
            answer.Title = answerStringList[i];
            answer.IsRight = isAnswerRightList[i];
            answer.Score = answerScoreList[i];
            answers.Add(answer);
        }
        SetAnswerList(answers);
    }

    
}
