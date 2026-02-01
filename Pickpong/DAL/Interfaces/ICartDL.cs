using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Interfaces
{
    public interface ICartDL
    {
        Task<TcartDetail?> GetUserCartAsync(int userId);
        Task<TcartDetail> CreateCartAsync(int userId);
        Task AddCarpetToCartAsync(TcarpetDetailsInCart carpetInCart);
        Task<List<TcartDetail>> GetCartsWithCarpetsAsync(int userId);
        Task<Dictionary<int, TcarpetDetail>> GetCarpetDetailsMapAsync(List<int> carpetIds);
        Task<List<CartDetails>> GetCarpetsInCurrentCartAsync(int userId);
        Task<Dictionary<int, decimal>> GetRegularCarpetPricesAsync(int userId);
        Task<decimal> CalculateCustomPriceAsync(CartDetails carpet);

    }
}
