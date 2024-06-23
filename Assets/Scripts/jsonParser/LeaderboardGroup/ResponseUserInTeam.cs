namespace ResponseUserInTeamJson
{
    using System.Collections;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Converters;
    using System.Globalization;

    public partial class ResponseUserInTeam
    {
        [JsonProperty("response")]
        public int ResponseCode { get; set; }
        [JsonProperty("data")]
        public List<ResponseUserInTeamItem> UserList { get; set; }
    }

    public partial class ResponseUserInTeam
    {
        public static ResponseUserInTeam FromJson(string json) => JsonConvert.DeserializeObject<ResponseUserInTeam>(json, ResponseUserInTeamJson.Converter.Settings);
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
