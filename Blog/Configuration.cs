namespace Blog;

public static class Configuration
{
    // TOKEN - Jason Web Token
    public static string JwtKey  = "ab09064f8d8d4a8ca870c5fd44a8737c=";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "curso_api_IlTevUM/Z0ey3nwCV/unWg==";

    public static SmtpConfiguration Smtp = new(); 

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
