using Microsoft.EntityFrameworkCore;
using Pickpong.DAL.Interfaces;
using Pickpong.Models;

namespace Pickpong.DAL.Classes
{
    public class BoardSettingsDL : IBoardSettingsDL
    {
        private readonly TexasTopContext _context;

        public BoardSettingsDL(TexasTopContext context)
        {
            _context = context;
        }

        public async Task<List<TboardSetting>> GetSizesByShapeIdAsync(int shapeId) =>
            await _context.TboardSettings
                          .Where(s => s.IIdShape == shapeId)
                          .ToListAsync()
                          .ConfigureAwait(false);

        public async Task<TboardSetting?> GetSettingByIdAsync(int id) =>
            await _context.TboardSettings
                          .FirstOrDefaultAsync(s => s.IIdSize == id)
                          .ConfigureAwait(false);

        public async Task<List<TboardSetting>> GetAllAsync() =>
            await _context.TboardSettings
                          .ToListAsync()
                          .ConfigureAwait(false);

        public async Task AddAsync(TboardSetting setting)
        {
            await _context.TboardSettings.AddAsync(setting).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(TboardSetting setting)
        {
            _context.TboardSettings.Update(setting);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            TboardSetting? setting = await GetSettingByIdAsync(id).ConfigureAwait(false);
            if (setting == null) return false;

            _context.TboardSettings.Remove(setting);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}