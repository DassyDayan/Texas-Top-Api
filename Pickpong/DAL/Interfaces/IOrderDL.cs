using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Interfaces
{
    public interface IOrderDL
    {
        Task<List<OrdersDetailModel>> GetOrdersDetailsAsync(int userId);
        Task<List<CartDetails>> GetAllOrdersAsync(int userId);
        Task<List<CartDetails>> GetOrdersWithRegularPricesAsync(int userId);
        Task<int> AddOrderAsync(Torder order);
        Task<int> GetCartIdByCarpetAsync(int userId, int carpetId);

        Task<TcarpetDetailsInCart?> GetCarpetInCartAsync(int carpetId);

        Task<Torder?> GetOrderByIdAsync(int orderId);
        Task<TcartDetail?> GetOpenCartForUserAsync(int userId);
        Task<TcartDetail> CreateCartForUserAsync(int userId);
        Task AddCarpetToCartAsync(TcarpetDetailsInCart carpetInCart);

        Task<TcustomerDetail> AddCustomerAsync(TcustomerDetail customer);
        Task<TcartDetail?> GetCartDetailsAsync(int cartId);
        Task<List<OrderGroupModel>> GetGroupedOrdersDetailsAsync(int userId);
        Task<List<OrdersDetailModel>> GetAllOrdersAsync();

        Task SaveChangesAsync();

    }
}
