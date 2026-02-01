namespace Pickpong.Models;

public partial class TcustomerDetail
{
    public int IIdCustomer { get; set; }

    public string? NvFirstName { get; set; }

    public string? NvLastName { get; set; }

    public string? NvPhoneNumber { get; set; }

    public string? NvEmailAddress { get; set; }

    public string? ISysTableRowIdEarth { get; set; }

    public string? ISysTableRowIdCity { get; set; }

    public string? ISysTableRowIdStreet { get; set; }

    public int? IBuildingNumber { get; set; }

    public int? IDepartmentNumber { get; set; }

    public string? NvZipCode { get; set; }

    public string? ISysTableRowIdPrefix { get; set; }

    public string? NvShippingNote { get; set; }

    public virtual ICollection<TcartDetail> TcartDetails { get; set; } = new List<TcartDetail>();
}