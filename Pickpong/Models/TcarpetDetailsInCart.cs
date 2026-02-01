namespace Pickpong.Models;

public partial class TcarpetDetailsInCart
{
    public int IIdCarpetDetailsInCart { get; set; }

    public int IIdCarpet { get; set; }

    public int IIdCart { get; set; }

    public int ICount { get; set; }

    public bool BDeleted { get; set; }

    public virtual TcarpetDetail IIdCarpetNavigation { get; set; } = null!;

    public virtual TcartDetail IIdCartNavigation { get; set; } = null!;
}
