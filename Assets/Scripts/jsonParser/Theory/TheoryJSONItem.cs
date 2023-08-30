using Newtonsoft.Json;

public class TheoryJSONItem
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("number")]
    public int Number { get; set; }

    [JsonProperty("chapter_name")]
    public string ChapterName { get; set; }

    [JsonProperty("name")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Content { get; set; }
}
