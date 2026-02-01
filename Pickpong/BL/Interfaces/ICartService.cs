using Pickpong.Dto.Classes;
using Pickpong.Entities;

namespace Pickpong.Services.Interfaces
{
    public interface ICartServiceBL
    {
        Task AddCarpetToUserCartAsync(int userId, int carpetId);
        Task<List<CartDetailsDto>> GetCartDetailsDtoAsync(int userId);
        Task<List<CartDetails>> GetCartDetailsAsync(int userId);
    }
}
