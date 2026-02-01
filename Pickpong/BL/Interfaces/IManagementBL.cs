using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.BL.Interfaces
{
    public interface IManagementBL
    {
        Task<List<List<OrdersDetailModel>>> GetAllOrdersAsync();
        Task<List<TboardSetting>?> GetSettingsAsync(int shapeId);
        Task<bool> UpdateSettingAsync(Settings setting);
        Task<bool> RemoveSettingAsync(int id);
        Task<List<Tcustomize>?> GetCustomizeSettingsAsync(int shapeId);
        Task<bool> UpdateCustomizeSettingAsync(SettingOfCustomize customize);
        Task<bool> MarkOrderAsSentAsync(int orderId);
        Task<int> AddSettingAsync(Settings setting);

    }
}
