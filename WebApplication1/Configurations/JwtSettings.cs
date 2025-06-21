namespace WebApplication1.Configurations
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public string Key { get; set; } = "";
        public int ExpiryMinutes { get; set; }
    }

}
