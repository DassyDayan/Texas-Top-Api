namespace Pickpong.Entities
{
    public class SettingOfCustomize
    {
        public int IIdCost { get; set; }
        public int IIdShape { get; set; }
        public decimal DMinLength { get; set; }
        public decimal DMinHeight { get; set; }
        public decimal DMaxLength { get; set; }
        public decimal DMaxHeight { get; set; }
        public decimal DPriceForMeter { get; set; }
    }
}