using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Interfaces
{
    public interface ICarpetDL
    {

        Task<CarpetDetailsResponse> GetCarpetDetailsAsync(int carpetId);
        Task<int> CreateCarpetEntryAsync(TcarpetDetail carpet);
        Task<List<TcartDetail>> GetCarpetsWithRegularPricesAsync(int orderId);
        Task<List<TcartDetail>> GetCarpetsInOrderAsync(int orderId);
        Task<List<TcartDetail>> GetCarpetsWithPricesAsync(int orderId);
        Task<TcarpetDetail?> GetByIdAsync(int carpetId);
        Task UpdateAsync(TcarpetDetail carpet);
        Task<List<TcarpetDetail>> GetCarpetsByIdsAsync(List<int> carpetIds);

    }
}
