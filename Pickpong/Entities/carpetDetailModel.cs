namespace Pickpong.Entities
{
    public class carpetDetailModel
    {
        public int? IIdCarpet { get; set; }
        public int IIdShape { get; set; }
        public int IIdColor { get; set; }
        public int INumOfPlayers { get; set; }
        public bool? BPutGuestOrEmptyPlace { get; set; }//
        public double FSizeParameterA { get; set; }
        public double FSizeParameterB { get; set; }
        public bool? BWithNamesOrNot { get; set; }//
        public string? NvLogoFileName { get; set; }
        public string? NvGroupName { get; set; }
        public string? NvCarpetFileName { get; set; }
        public string? NvSmallCarpetFileName { get; set; }
        public string? ShapeImgUrl { get; set; }//ניתוב לתמונה בפרויקט של צבע וצורה של שטיח
        public List<PlayerDTO> Players { get; set; } = new List<PlayerDTO>();
    }
}