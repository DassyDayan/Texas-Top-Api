namespace Pickpong.Entities
{
    public class ExternalLoginModel
    {
        public string Provider { get; set; } = string.Empty;
        public string ExternalId { get; set; } = string.Empty;
        public string? FullName { get; set; }
    }

}
