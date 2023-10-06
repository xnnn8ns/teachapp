using Newtonsoft.Json;

public class TestJSONItem
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("difficulty")]
    public int Difficulty { get; set; }

    [JsonProperty("points")]
    public int Points { get; set; }

    [JsonProperty("question")]
    public string Question { get; set; }

    [JsonProperty("correct_answer")]
    public int CorrectAnswer { get; set; }

    [JsonProperty("answer_1")]
    public string Answer1 { get; set; }

    [JsonProperty("answer_2")]
    public string Answer2 { get; set; }

    [JsonProperty("answer_3")]
    public string Answer3 { get; set; }

    [JsonProperty("answer_4")]
    public string Answer4 { get; set; }

    [JsonProperty("topic")]
    public int Topic { get; set; }

    [JsonProperty("step")]
    public int Step { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }
}
