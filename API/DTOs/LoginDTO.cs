namespace MyWebsite.DTOs
{
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public DateTime Expire { get; set; }

    }
}
