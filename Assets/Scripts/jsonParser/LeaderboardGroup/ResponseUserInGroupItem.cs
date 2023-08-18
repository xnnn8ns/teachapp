using Newtonsoft.Json;

public class ResponseUserInGroupItem
{
    [JsonProperty("UserID")]
    public int UserID { get; set; }

    [JsonProperty("UserFullName")]
    public string UserFullName { get; set; }

    [JsonProperty("UserEmail")]
    public string UserEmail { get; set; }

    [JsonProperty("UserAvatarID")]
    public int UserAvatarID { get; set; }

    [JsonProperty("IsByVK")]
    public int IsByVK { get; set; }

    [JsonProperty("VKID")]
    public int VKID { get; set; }

    [JsonProperty("Score")]
    public int Score { get; set; }
}
