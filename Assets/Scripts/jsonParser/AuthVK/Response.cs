using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Response
{
    [JsonProperty("id")]
    public string UserID { get; set; }

    [JsonProperty("photo_50")]
    public string UserPhoto { get; set; }

    [JsonProperty("first_name")]
    public string UserFirstName { get; set; }

    [JsonProperty("last_name")]
    public string UserLastName { get; set; }
}
