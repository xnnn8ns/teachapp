using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class RawDataLevelList
{
    //https://jsonblob.com/1079849597828087808
    //http://jsonblob.com/1110974511666446336
    [JsonProperty("levels")] public List<RawDataLevel> RawLevels = new List<RawDataLevel>();
}

[Serializable]
public class RawDataLevel
{
    [JsonProperty("level")] public int Level = 1;
    [JsonProperty("totalTime")] public int TotalTime;
    [JsonProperty("totalScore")] public int TotalScore;
    [JsonProperty("questions")] public List<RawQuestion> RawQuestions = new List<RawQuestion>();
}

[Serializable]
public class RawQuestion
{
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
