namespace Pickpong.Dto.Classes
{
    public class UpdateCarpetCountRequest
    {
        public int IIdCarpet { get; set; }
        public int CountChange { get; set; }
    }

    public enum UpdateCountResult
    {
        Success,
        NotFound,
        InvalidCount
    }

    public class OrderAgainRequest
    {
        public int IidCarpet { get; set; }
    }

}