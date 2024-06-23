using Newtonsoft.Json;

public class TaskJSONItem
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("difficulty")]
    public int Difficulty { get; set; }

    [JsonProperty("blocks")]
    public string Blocks { get; set; }

    [JsonProperty("solution")]
    public string Content { get; set; }

    [JsonProperty("solution_eng")]
    public string ContentEn { get; set; }

    [JsonProperty("points")]
    public int Points { get; set; }

    [JsonProperty("description")]
    public string Title { get; set; }

    [JsonProperty("description_eng")]
    public string TitleEn { get; set; }

    [JsonProperty("topic")]
    public int Topic { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }

    [JsonProperty("step")]
    public int Step { get; set; }

    [JsonProperty("additionalBlocks")]
    public string AdditionalBlocks { get; set; }
}
