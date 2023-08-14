using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ResponseAuth
{
    [JsonProperty("UserID")]
    public int UserID { get; set; }

    [JsonProperty("UserFullName")]
    public string UserFullName { get; set; }

    [JsonProperty("DateRegistry")]
    public string DateRegistry { get; set; }

    [JsonProperty("Password")]
    public string UserPassword { get; set; }

    [JsonProperty("UserEmail")]
    public string UserEmail { get; set; }

    [JsonProperty("UserAvatarID")]
    public int UserAvatarID { get; set; }

    [JsonProperty("IsByVK")]
    public int IsByVK { get; set; }

    [JsonProperty("VKID")]
    public int VKID { get; set; }
}
