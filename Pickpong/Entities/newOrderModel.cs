namespace Pickpong.Entities
{
    public class newOrderModel
    {

        //customer
        public string? NvFirstName { get; set; }

        public string? NvLastName { get; set; }

        public string? NvPhoneNumber { get; set; }

        public string? NvEmailAddress { get; set; }

        public string? ISysTableRowIdCity { get; set; }

        public string? ISysTableRowIdEarth { get; set; }

        public string? ISysTableRowIdStreet { get; set; }

        public int? IBuildingNumber { get; set; }

        public int? IDepartmentNumber { get; set; }

        public string? NvZipCode { get; set; }

        //order
        public DateTime DtOrderDate { get; set; }

        public string? NvShippingNote { get; set; }

        public string NvOrderNumber { get; set; } = null!;

        public int IIdCart { get; set; }

        public int ISysTableRowIdShippingType { get; set; }

        public int ISysTableRowIdOrderStatus { get; set; }

        public int IIdInvoice { get; set; }

        public decimal DPrice { get; set; }

        public int ISysTableRowIdPaymentType { get; set; }

        public string? ISysTableRowIdPrefix { get; set; }
    }
}