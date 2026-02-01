using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Classes
{
    public class PlayerDL : IPlayerDL
    {
        private readonly TexasTopContext _context;

        public PlayerDL(TexasTopContext context)
        {
            _context = context;
        }

        public async Task AddPlayersToCarpetAsync(List<PlayerDTO> players, int carpetId)
        {
            IEnumerable<Tplayer> entities = players.Select((p, i) => new Tplayer
            {
                IIdCarpet = carpetId,
                IPlace = i + 1,
                NvName = p.Name,
                NvNickName = p.NickName
            });

            await _context.Tplayers.AddRangeAsync(entities).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}