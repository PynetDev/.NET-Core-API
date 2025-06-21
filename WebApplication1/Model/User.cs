namespace WebApplication1.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // In production, never store raw passwords
        public string Role { get; set; }
    }
}
