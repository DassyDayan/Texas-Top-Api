using Microsoft.EntityFrameworkCore;
using Pickpong.DAL.Interfaces;
using Pickpong.Models;

namespace Pickpong.DAL.Classes
{
    public class CustomizesDL : ICustomizesDL
    {
        private readonly TexasTopContext _context;

        public CustomizesDL(TexasTopContext context)
        {
            _context = context;
        }

        public async Task<List<Tcustomize>> GetCustomizeSizesByShapeIdAsync(int shapeId)
        {
            return await _context.Tcustomizes
                .Where(c => c.IIdShape == shapeId)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<decimal> CalculateCustomPriceAsync(int shapeId, decimal sizeA, decimal sizeB)
        {
            return await _context.Tcustomizes
                .Where(c => c.IIdShape == shapeId)
                .Select(c =>
                    Convert.ToDecimal(c.DPriceForMeter * (sizeA / 100) * (sizeB / 100)))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Tcustomize>> GetCustomizeSettingsAsync(int shapeId)
        {
            return await _context.Tcustomizes
                .Where(c => c.IIdShape == shapeId)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Tcustomize?> GetCustomizeSettingAsync(int shapeId)
        {
            return await _context.Tcustomizes
                .FirstOrDefaultAsync(c => c.IIdShape == shapeId)
                .ConfigureAwait(false);
        }

        public async Task UpdateCustomizeSettingAsync(Tcustomize customize)
        {
            _context.Tcustomizes.Update(customize);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}