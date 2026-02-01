namespace Pickpong.Dto.Classes
{
    public class OrderStatusUpdateRequest
    {
        public int IIdOrder { get; set; }
    }

    public enum OrderStatusUpdateResult
    {
        Success,
        NotFound,
        InvalidState
    }

}