namespace Pickpong.Dto.Classes
{
    public class CartDetailsDto
    {
        public int IIdCart { get; set; }
        public bool BStatusPaid { get; set; }
        public List<CarpetDetailDto> Carpets { get; set; } = new();
    }
}