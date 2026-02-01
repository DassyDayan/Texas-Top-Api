namespace Pickpong.Models;

public partial class TsysTableRow
{
    public int IIdSysTableRow { get; set; }
    public int IIdSysTable { get; set; }
    public string NvValueEng { get; set; } = null!;
    public string NvValueHeb { get; set; } = null!;
    public virtual TsysTable IIdSysTableNavigation { get; set; } = null!;
    public virtual ICollection<Torder> TorderISysTableRowIdOrderStatusNavigations { get; set; } =
        new List<Torder>();
    public virtual ICollection<Torder> TorderISysTableRowIdPaymentTypeNavigations { get; set; } =
        new List<Torder>();
    public virtual ICollection<Torder> TorderISysTableRowIdShippingTypeNavigations { get; set; } =
        new List<Torder>();
}