using Newtonsoft.Json;

public class ResponseTeamItem
{
    [JsonProperty("TeamID")]
    public int TeamID { get; set; }

    [JsonProperty("TeamName")]
    public string TeamName { get; set; }

    [JsonProperty("UserIDAdmin")]
    public int UserIDAdmin { get; set; }
}
