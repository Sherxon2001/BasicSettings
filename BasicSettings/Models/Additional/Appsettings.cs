namespace BasicSettings.Models.Additional
{
    public class Appsettings
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public AuthSettings AuthSettings { get; set; }
        public SwaggerSetting SwaggerSetting { get; set; }
    }

    public class SwaggerSetting
    {
        public bool PrefixIsEnable { get; set; }
        public string Prefix { get; set; }
    }

    public class AuthSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SecretKey { get; set; }
        public int Expires { get; set; }
    }

    public class ConnectionStrings
    {
        public string ConnectionString { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }

        [JsonProperty("Microsoft.AspNetCore")]
        public string MicrosoftAspNetCore { get; set; }
    }
}
