namespace AuthJsonNew
{
    using System.Collections;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Converters;
    using System.Globalization;

    public partial class ResponseAuthNew
    {
        [JsonProperty("id")]
        public string UserID { get; set; }

        [JsonProperty("UserFullName")]
        public string UserFullName { get; set; }

        [JsonProperty("DateRegistry")]
        public string DateRegistry { get; set; }

        [JsonProperty("Password")]
        public string UserPassword { get; set; }

        [JsonProperty("login")]
        public string UserEmail { get; set; }

        [JsonProperty("UserAvatarID")]
        public int UserAvatarID { get; set; }

        [JsonProperty("IsByVK")]
        public int IsByVK { get; set; }

        [JsonProperty("VKID")]
        public int VKID { get; set; }

        [JsonProperty("Score")]
        public int Score { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("refresh_token_expiration")]
        public long RefreshTokenExpiration { get; set; }
    }

    public partial class ResponseAuthNew
    {
        public static ResponseAuthNew FromJson(string json) => JsonConvert.DeserializeObject<ResponseAuthNew>(json, AuthJsonNew.Converter.Settings);
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
