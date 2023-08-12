namespace VKJson
{

    using System.Collections;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Converters;
    using System.Globalization;

    public partial class VKResponse
    {
        [JsonProperty("response")]
        public List<Response> ResponseList { get; set; }
    }

    public partial class VKResponse
    {
        public static VKResponse FromJson(string json) => JsonConvert.DeserializeObject<VKResponse>(json, VKJson.Converter.Settings);
    }

    internal static class Converter
    {
        static List<string> errors = new List<string>();

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Error = delegate (object sender, ErrorEventArgs args)
            {
                errors.Add(args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
                //  Debug.LogError(args.ErrorContext.Error.Message);
            },
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

}
