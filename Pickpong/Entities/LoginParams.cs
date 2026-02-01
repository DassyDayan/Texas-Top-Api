namespace Pickpong.Entities
{
    public class LoginParams
    {
        public string mobileOrMail { get; set; } = string.Empty;
        public string? OTP { get; set; }
        public bool isMail { get; set; }
    }
}