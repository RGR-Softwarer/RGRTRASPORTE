namespace Dominio.Models.Integration
{
    public class IntegrationConfig
    {
        public AuthConfig Auth { get; set; }
        public IntegrationDetails Integration { get; set; }
    }

    public class AuthConfig
    {
        public AuthType Type { get; set; }
        public Dictionary<string, string> Credentials { get; set; }
        public string TokenHeaderName { get; set; }
        public string TokenHeaderFormat { get; set; }
        public string TokenHeaderNameToSend { get; set; }
        public string TokenHeaderFormatToSend { get; set; }
        public string AuthUrl { get; set; }
        public string AuthMethod { get; set; }
        public string TokenResponseField { get; set; }
    }

    public class IntegrationDetails
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> PayloadTemplate { get; set; }
    }

    public enum AuthType
    {
        None = 0,
        Basic = 1,
        ApiKey = 2,
        OAuth2 = 3
    }
} 
