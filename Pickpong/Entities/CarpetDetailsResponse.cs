using Pickpong.Models;

namespace Pickpong.Entities
{
    public class CarpetDetailsResponse
    {
        public List<TcarpetDetail> CarpetDetails { get; set; } = new List<TcarpetDetail>();
        public List<Tplayer> Players { get; set; } = new List<Tplayer>();
    }
}