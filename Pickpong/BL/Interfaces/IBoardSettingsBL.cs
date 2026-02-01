using Pickpong.Entities;

namespace Pickpong.BL.Interfaces
{
    public interface IBoardSettingsBL
    {
        Task<List<SizeOptionsDto>?> GetSizesByShapeIdAsync(int idShape);
    }
}