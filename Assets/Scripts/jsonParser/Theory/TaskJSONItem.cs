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

    [JsonProperty("points")]
    public int Points { get; set; }

    [JsonProperty("description")]
    public string Title { get; set; }

    [JsonProperty("topic")]
    public int Topic { get; set; }
}
