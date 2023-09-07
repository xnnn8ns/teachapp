using Newtonsoft.Json;

public class TheoryListItemJSON
{
    [JsonProperty("id")]
    public int ID { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

}
