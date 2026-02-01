using Microsoft.EntityFrameworkCore;
using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Classes
{
    public class LoginDL : ILoginDL
    {
        private readonly TexasTopContext _context;

        public LoginDL(TexasTopContext context)
        {
            _context = context;
        }

        public async Task<TcustomerDetail> GetOrCreateCustomerAsync(LoginParams loginParams)
        {
            TcustomerDetail? customer = await _context.TcustomerDetails
                .FirstOrDefaultAsync(c =>
                    c.NvEmailAddress == loginParams.mobileOrMail ||
                    c.NvPhoneNumber == loginParams.mobileOrMail)
                .ConfigureAwait(false);

            if (customer == null)
            {
                customer = new TcustomerDetail();

                if (loginParams.isMail)
                    customer.NvEmailAddress = loginParams.mobileOrMail;
                else
                    customer.NvPhoneNumber = loginParams.mobileOrMail;

                await _context.TcustomerDetails.AddAsync(customer).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return customer;
        }

        public async Task<TcustomerDetail?> GetCustomerByEmailOrPhoneAsync(string mobileOrMail)
        {
            return await _context.TcustomerDetails
                .FirstOrDefaultAsync(x =>
                    x.NvPhoneNumber == mobileOrMail ||
                    x.NvEmailAddress == mobileOrMail)
                .ConfigureAwait(false);
        }

        public async Task<TcustomerDetail?> GetCustomerByIdAsync(int customerId)
        {
            return await _context.TcustomerDetails
                .FirstOrDefaultAsync(x => x.IIdCustomer == customerId)
                .ConfigureAwait(false);
        }

        public async Task<int?> GetLatestOrderIdByCustomerIdAsync(int customerId)
        {
            return await (
                from order in _context.Torders
                join cart in _context.TcartDetails
                    on order.IIdCart equals cart.IIdCart
                where cart.IIdCustomer == customerId
                orderby order.DtOrderDate descending
                select order.IIdOrder
            ).FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}