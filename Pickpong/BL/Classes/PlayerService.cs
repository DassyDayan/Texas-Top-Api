using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Services.Interfaces;

public class PlayerServiceBL : IPlayerServiceBL
{
    private readonly IPlayerDL _playerDL;

    public PlayerServiceBL(IPlayerDL playerDL)
    {
        _playerDL = playerDL;
    }

    public async Task AddPlayersToCarpetAsync(List<PlayerDTO>? players, int carpetId)
    {
        if (players == null || !players.Any()) return;
        await _playerDL.AddPlayersToCarpetAsync(players, carpetId);
    }
}