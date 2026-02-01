namespace Pickpong.Models;

public partial class TcartDetail
{
    public int IIdCart { get; set; }

    public int IIdCustomer { get; set; }

    public bool BStatusPaid { get; set; }

    public virtual TcustomerDetail IIdCustomerNavigation { get; set; } = null!;

    public virtual ICollection<TcarpetDetailsInCart> TcarpetDetailsInCarts { get; set; } = new List<TcarpetDetailsInCart>();

    public virtual ICollection<Torder> Torders { get; set; } = new List<Torder>();
}