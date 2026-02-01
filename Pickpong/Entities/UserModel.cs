using Pickpong.Models;

namespace Pickpong.Entities
{
    public class UserModel1
    {
        public int IIdCustomer { get; set; }

        public string? NvFirstName { get; set; }

        public string? NvLastName { get; set; }

        public string? NvPhoneNumber { get; set; }

        public string? NvEmailAddress { get; set; }

        public int? ISysTableRowIdEarth { get; set; }

        public int? ISysTableRowIdCity { get; set; }

        public int? ISysTableRowIdStreet { get; set; }

        public int? IBuildingNumber { get; set; }

        public int? IDepartmentNumber { get; set; }

        public string? NvZipCode { get; set; }

        public int? ISysTableRowIdPrefix { get; set; }

        public string? NvShippingNote { get; set; }

        public virtual ICollection<TcartDetail> TcartDetails { get; set; } =
            new List<TcartDetail>();
    }
}