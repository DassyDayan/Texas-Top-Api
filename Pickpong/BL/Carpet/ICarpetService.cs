using Pickpong.Dto.Classes;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.BL.Carpet
{
    public interface ICarpetService
    {
        Task<Result<List<CartDetailsDto>>> CreateCarpetAsync(carpetDetailModel carpetModel, int userId);
        Task<CarpetDetailsResponse> GetCarpetDetailsAsync(int carpetId);
        Task<List<CartDetails>> GetCarpetsWithPricesAsync(int orderId);
        Task<List<CartDetails>> GetCarpetsInOrderAsync(int orderId);
        Task SetDefaultFileNamesAsync(int carpetId);
        Task<List<TcarpetDetail>> GetCarpetsByIdsAsync(List<int> carpetIds);
    }
}
