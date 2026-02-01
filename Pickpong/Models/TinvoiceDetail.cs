namespace Pickpong.Models;

public partial class TinvoiceDetail
{
    public int IIdInvoice { get; set; }

    public string NvDealerName { get; set; } = null!;

    public string NvDealerNumber { get; set; } = null!;

    public string? NvPhoneNumber { get; set; }

    public string? NvEmailAddress { get; set; }

    public string? ISysTableRowIdEarth { get; set; }

    public string? ISysTableRowIdCity { get; set; }

    public string? ISysTableRowIdStreet { get; set; }

    public int? IBuildingNumber { get; set; }

    public int? IDepartmentNumber { get; set; }

    public string? NvZipCode { get; set; }

    public virtual ICollection<Torder> Torders { get; set; } = new List<Torder>();
}