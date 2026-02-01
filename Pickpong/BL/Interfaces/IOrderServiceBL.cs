using Pickpong.BL.Classes;
using Pickpong.Dto.Classes;
using Pickpong.Entities;
using UpdateCarpetCountRequest = Pickpong.Dto.Classes.UpdateCarpetCountRequest;

namespace Pickpong.BL.Interfaces
{
    public interface IOrderServiceBL
    {
        Task<List<CartDetails>> GetOrderDetailsWithPricesAsync(int orderId);

        Task<List<List<OrdersDetailModel>>> GetGroupedOrdersDetailsAsync(int userId);
        Task<List<CartDetails>> GetOrdersDetailsAsync(int userId);
        Task<int> CreateNewOrderAsync(int userId, int carpetId);

        Task<DecreaseResult> DecreaseCarpetCountAsync(int carpetId);

        Task<UpdateCountResult> UpdateCarpetCountAsync(UpdateCarpetCountRequest request);
        Task<OrderStatusUpdateResult> MarkOrderAsReceivedAsync(int orderId);

        Task<bool> OrderCarpetAgainAsync(int userId, int carpetId);
        Task<int> SaveOrderAsync(newOrderModel order);
    }
}
