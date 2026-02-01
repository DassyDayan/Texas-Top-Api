namespace Pickpong.Entities
{
    public class UpdateCarpetCountRequest
    {
        public int IIdCarpet { get; set; }
        public int CountChange { get; set; } // +1 להוספה, -1 להפחתה
    }
}
