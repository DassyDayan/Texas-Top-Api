namespace Pickpong.Models;

public partial class TcarpetDetail
{
    public int IIdCarpet { get; set; }

    public int IIdShape { get; set; }

    public int IIdColor { get; set; }

    public int INumOfPlayers { get; set; }

    public bool BPutGuestOrEmptyPlace { get; set; }

    public double FSizeParameterA { get; set; }

    public double FSizeParameterB { get; set; }

    public bool BWithNamesOrNot { get; set; }

    public string? NvLogoFileName { get; set; }

    public string? NvGroupName { get; set; }

    public string? NvCarpetFileName { get; set; }

    public string? NvSmallCarpetFileName { get; set; }

    public virtual Tcolor IIdColorNavigation { get; set; } = null!;

    public virtual Tshape IIdShapeNavigation { get; set; } = null!;

    public virtual ICollection<TcarpetDetailsInCart> TcarpetDetailsInCarts { get; set; }
        = new List<TcarpetDetailsInCart>();

    public virtual ICollection<Tplayer> Tplayers { get; set; } = new List<Tplayer>();
}