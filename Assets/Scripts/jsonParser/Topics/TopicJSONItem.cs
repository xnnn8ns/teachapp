using Newtonsoft.Json;

public class TopicJSONItem
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("number")]
    public int Number { get; set; }

    [JsonProperty("name")]
    public string Title { get; set; }

    [JsonProperty("name_eng")]
    public string TitleEn { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("description_eng")]
    public string DescriptionEn { get; set; }

}
