using Pickpong.Models;

namespace Pickpong.DAL.Interfaces
{
    public interface ICustomizesDL
    {
        Task<List<Tcustomize>> GetCustomizeSizesByShapeIdAsync(int idShape);
        Task<decimal> CalculateCustomPriceAsync(int shapeId, decimal sizeA, decimal sizeB);
        Task<List<Tcustomize>> GetCustomizeSettingsAsync(int shapeId);
        Task<Tcustomize?> GetCustomizeSettingAsync(int shapeId);
        Task UpdateCustomizeSettingAsync(Tcustomize customize);
    }
}
