namespace Pickpong.Models;

public partial class Tplayer
{
    public int IIdPlayer { get; set; }
    public int IIdCarpet { get; set; }
    public int IPlace { get; set; }
    public string NvName { get; set; } = null!;
    public string NvNickName { get; set; } = null!;
    public virtual TcarpetDetail IIdCarpetNavigation { get; set; } = null!;
}