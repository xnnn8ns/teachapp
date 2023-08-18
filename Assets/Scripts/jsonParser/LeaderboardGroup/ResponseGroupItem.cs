using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ResponseGroupItem
{
    [JsonProperty("GroupID")]
    public int GroupID { get; set; }
}
