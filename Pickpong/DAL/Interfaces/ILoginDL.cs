using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Interfaces
{
    public interface ILoginDL
    {
        Task<TcustomerDetail> GetOrCreateCustomerAsync(LoginParams loginParams);
        Task<TcustomerDetail?> GetCustomerByEmailOrPhoneAsync(string mobileOrMail);
        Task<TcustomerDetail?> GetCustomerByIdAsync(int customerId);
        Task<int?> GetLatestOrderIdByCustomerIdAsync(int customerId);
    }
}
