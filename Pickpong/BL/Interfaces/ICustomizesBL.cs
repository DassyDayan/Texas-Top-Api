using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.BL.Interfaces
{
    public interface ICustomizesBL
    {
        Task<List<CustomizeSettingDTO>?> GetCustomizeSizesByShapeIdAsync(int idShape);
        Task<Result<decimal>> CalculateCustomPriceAsync(CartDetails carpet);
    }
}