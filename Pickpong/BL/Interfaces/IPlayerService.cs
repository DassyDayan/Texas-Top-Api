using Pickpong.Entities;

namespace Pickpong.Services.Interfaces
{
    public interface IPlayerServiceBL
    {
        Task AddPlayersToCarpetAsync(List<PlayerDTO>? players, int carpetId);
    }
}
