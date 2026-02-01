namespace Pickpong.Entities
{
    public class CartDetails
    {
        public int? IIdCarpet { get; set; }
        public int? IIdShape { get; set; }
        public int? IIdColor { get; set; }
        public double? FSizeParameterA { get; set; }
        public double? FSizeParameterB { get; set; }
        public decimal? DPrice { get; set; }
        public int? ICount { get; set; }
        public string? NvCarpetFileName { get; set; } = null!;
        public string? NvSmallCarpetFileName { get; set; } = null!;
    }
}