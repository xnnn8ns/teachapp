using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class RawDataQuestion
{
    //https://jsonblob.com/1079849597828087808
    [JsonProperty("questions")] public List<RawQuestion> RawQuestions = new List<RawQuestion>();
}

[Serializable]
public class RawQuestion
{
    [JsonProperty("level")] public int Level = 1;
    [JsonProperty("title")] public string Title;
    [JsonProperty("rawAnswers")] public List<RawAnswer> RawAnswers = new List<RawAnswer>();
    [JsonProperty("type")] public int QuestionType;
    [JsonProperty("countShelves")] public int CountShelves = 1;
    [JsonProperty("score")] public int Score = 0;
    [JsonProperty("isSingleRightAnswer")] public bool IsSingleRightAnswer = false;
}

[Serializable]
public class RawAnswer
{
    [JsonProperty("title")] public string Title;
    [JsonProperty("isRight")] public bool IsRight = false;
    [JsonProperty("score")] public int Score = 0;
    [JsonProperty("isPositionDependent")] public bool IsPositionDependent = false;
    [JsonProperty("positionRowIndex")] public int PositionRowIndex = 0;
    [JsonProperty("positionCellIndex")] public int PositionCellIndex = 0;
}
