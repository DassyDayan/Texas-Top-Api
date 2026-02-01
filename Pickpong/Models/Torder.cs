namespace Pickpong.Models;

public partial class Torder
{
    public int IIdOrder { get; set; }

    public string NvOrderNumber { get; set; } = null!;

    public DateTime DtOrderDate { get; set; }

    public int IIdCart { get; set; }

    public int ISysTableRowIdShippingType { get; set; }

    public int ISysTableRowIdOrderStatus { get; set; }

    public int IIdInvoice { get; set; }

    public decimal DPrice { get; set; }

    public int ISysTableRowIdPaymentType { get; set; }

    public string? NvShippingNote { get; set; }

    public virtual TcartDetail IIdCartNavigation { get; set; } = null!;

    public virtual TinvoiceDetail IIdInvoiceNavigation { get; set; } = null!;

    public virtual TsysTableRow ISysTableRowIdOrderStatusNavigation { get; set; } = null!;

    public virtual TsysTableRow ISysTableRowIdPaymentTypeNavigation { get; set; } = null!;

    public virtual TsysTableRow ISysTableRowIdShippingTypeNavigation { get; set; } = null!;
}