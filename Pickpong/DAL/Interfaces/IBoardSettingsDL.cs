using Pickpong.Models;

namespace Pickpong.DAL.Interfaces
{
    public interface IBoardSettingsDL
    {
        Task<List<TboardSetting>> GetSizesByShapeIdAsync(int shapeId);

        Task<TboardSetting?> GetSettingByIdAsync(int id);

        Task<List<TboardSetting>> GetAllAsync();

        Task AddAsync(TboardSetting setting);

        Task UpdateAsync(TboardSetting setting);

        Task<bool> RemoveAsync(int id);
    }
}