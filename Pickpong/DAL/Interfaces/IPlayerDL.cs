using Pickpong.Entities;

namespace Pickpong.DAL.Interfaces
{
    public interface IPlayerDL
    {
        Task AddPlayersToCarpetAsync(List<PlayerDTO> players, int carpetId);
    }
}