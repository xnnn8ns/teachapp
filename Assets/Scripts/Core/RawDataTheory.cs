using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class RawDataTheory
{
    //http://jsonblob.com/1095423123372916736
    [JsonProperty("theory")] public List<RawTheory> RawTheories = new List<RawTheory>();
}

[Serializable]
public class RawTheory
{
    [JsonProperty("title")] public string Title;
    [JsonProperty("texts")] public List<string> RawTexts = new List<string>();
}
