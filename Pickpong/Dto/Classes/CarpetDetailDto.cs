using Pickpong.Models;

namespace Pickpong.Dto.Classes
{
    public class CarpetDetailDto
    {
        public int IIdCarpet { get; set; }
        public int ICount { get; set; }
        public TcarpetDetail CarpetDetail { get; set; } = new TcarpetDetail();
    }
}