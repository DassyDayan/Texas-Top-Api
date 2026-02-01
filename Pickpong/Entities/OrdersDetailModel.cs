namespace Pickpong.Entities
{
    public class OrdersDetailModel
    {
        //TcarpetDetailsInCart:
        public int IidCarpet { get; set; }
        public int ICount { get; set; } = 0;
        public string NvCarpetFileName { get; set; } = null!;


        //Torder:
        public int? IIdOrder { get; set; }
        public string NvOrderNumber { get; set; } = null!;
        public DateTime DtOrderDate { get; set; }
        public int ISysTableRowIdShippingType { get; set; }
        public int ISysTableRowIdOrderStatus { get; set; }
        public decimal DPrice { get; set; }
        public int ISysTableRowIdPaymentType { get; set; }


        //TcustomerDetail:
        public string? NvFirstName { get; set; }
        public string? NvLastName { get; set; }
        public string? NvPhoneNumber { get; set; }
        public string? ISysTableRowIdCity { get; set; }
        public string? ISysTableRowIdStreet { get; set; }
        public int? IBuildingNumber { get; set; }
        public int? IDepartmentNumber { get; set; }
        public string? NvZipCode { get; set; }


        //TinvoiceDetail:
        public string NvDealerName { get; set; } = null!;
        public string NvDealerNumber { get; set; } = null!;
        public string? DISysTableRowIdCity { get; set; }
        public string? DISysTableRowIdStreet { get; set; }
        public int? DIBuildingNumber { get; set; }
        public int? DIDepartmentNumber { get; set; }
        public string? DNvZipCode { get; set; }
    }
}