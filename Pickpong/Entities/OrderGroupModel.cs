namespace Pickpong.Entities
{
    public class OrderGroupModel
    {
        public int OrderId { get; set; }
        public string NvOrderNumber { get; set; } = string.Empty;
        public DateTime DtOrderDate { get; set; }
        public List<OrdersDetailModel> Items { get; set; } = new();
    }
}
