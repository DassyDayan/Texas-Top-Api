using Microsoft.EntityFrameworkCore;
using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Classes
{
    public class CarpetDL : ICarpetDL
    {
        private readonly TexasTopContext _context;

        public CarpetDL(TexasTopContext context)
        {
            _context = context;
        }

        #region Get Carpet Details

        public async Task<CarpetDetailsResponse> GetCarpetDetailsAsync(int carpetId)
        {
            List<TcarpetDetail> carpetDetails = await GetCarpetDetailsFromDbAsync(carpetId).ConfigureAwait(false);
            List<Tplayer>? players = await GetPlayersForCarpetAsync(carpetId).ConfigureAwait(false);

            return new CarpetDetailsResponse
            {
                CarpetDetails = carpetDetails,
                Players = players
            };
        }

        private async Task<List<TcarpetDetail>> GetCarpetDetailsFromDbAsync(int carpetId)
        {
            return await (
                from carpet in _context.TcarpetDetails
                join boardSetting in _context.TboardSettings
                    on new
                    {
                        Length = Convert.ToDecimal(carpet.FSizeParameterA),
                        Width = Convert.ToDecimal(carpet.FSizeParameterB)
                    }
                    equals new
                    {
                        Length = boardSetting.DLength,
                        Width = boardSetting.DHeight
                    }
                where carpet.IIdCarpet == carpetId
                select new TcarpetDetail
                {
                    IIdCarpet = carpet.IIdCarpet,
                    IIdShape = carpet.IIdShape,
                    IIdColor = carpet.IIdColor,
                    INumOfPlayers = carpet.INumOfPlayers,
                    FSizeParameterA = carpet.FSizeParameterA,
                    FSizeParameterB = carpet.FSizeParameterB,
                    BPutGuestOrEmptyPlace = carpet.BPutGuestOrEmptyPlace,
                    BWithNamesOrNot = carpet.BWithNamesOrNot,
                    NvCarpetFileName = NullIfEmpty(carpet.NvCarpetFileName),
                    NvSmallCarpetFileName = NullIfEmpty(carpet.NvSmallCarpetFileName),
                    NvGroupName = NullIfEmpty(carpet.NvGroupName),
                    NvLogoFileName = NullIfEmpty(carpet.NvLogoFileName)
                }
            ).ToListAsync().ConfigureAwait(false);
        }

        private async Task<List<Tplayer>> GetPlayersForCarpetAsync(int carpetId)
        {
            return await _context.Tplayers
                .Where(p => p.IIdCarpet == carpetId)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private static string? NullIfEmpty(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value;

        #endregion

        #region CRUD

        public async Task<int> CreateCarpetEntryAsync(TcarpetDetail carpet)
        {
            await _context.TcarpetDetails.AddAsync(carpet).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return carpet.IIdCarpet;
        }

        public async Task<TcarpetDetail?> GetByIdAsync(int carpetId)
        {
            return await _context.TcarpetDetails
                .FirstOrDefaultAsync(c => c.IIdCarpet == carpetId)
                .ConfigureAwait(false);
        }

        public async Task UpdateAsync(TcarpetDetail carpet)
        {
            _context.TcarpetDetails.Update(carpet);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<List<TcarpetDetail>> GetCarpetsByIdsAsync(List<int> carpetIds)
        {
            return await _context.TcarpetDetails
                .Where(c => carpetIds.Contains(c.IIdCarpet))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        #endregion

        #region Orders & CartDetails

        public async Task<List<TcartDetail>> GetCarpetsWithRegularPricesAsync(int orderId)
        {
            return await GetCarpetsWithSettingsQuery(orderId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<TcartDetail>> GetCarpetsWithPricesAsync(int orderId)
        {
            return await GetBasicCarpetsQuery(orderId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<TcartDetail>> GetCarpetsInOrderAsync(int orderId)
        {
            return await GetBasicCarpetsQuery(orderId).ToListAsync().ConfigureAwait(false);
        }

        private IQueryable<TcartDetail> GetBasicCarpetsQuery(int orderId)
        {
            return from cartDetails in _context.TcartDetails
                   join carpetInCart in _context.TcarpetDetailsInCarts
                       on cartDetails.IIdCart equals carpetInCart.IIdCart
                   join carpet in _context.TcarpetDetails
                       on carpetInCart.IIdCarpet equals carpet.IIdCarpet
                   join order in _context.Torders
                       on cartDetails.IIdCart equals order.IIdCart
                   where order.IIdCart == orderId && carpetInCart.BDeleted != true
                   select cartDetails;
        }

        private IQueryable<TcartDetail> GetCarpetsWithSettingsQuery(int orderId)
        {
            return from cartDetails in _context.TcartDetails
                   join carpetInCart in _context.TcarpetDetailsInCarts
                       on cartDetails.IIdCart equals carpetInCart.IIdCart
                   join carpet in _context.TcarpetDetails
                       on carpetInCart.IIdCarpet equals carpet.IIdCarpet
                   join setting in _context.TboardSettings
                       on new
                       {
                           ShapeId = carpet.IIdShape,
                           Length = Convert.ToDecimal(carpet.FSizeParameterA),
                           Width = Convert.ToDecimal(carpet.FSizeParameterB)
                       }
                       equals new
                       {
                           ShapeId = setting.IIdShape,
                           Length = setting.DLength,
                           Width = setting.DHeight
                       }
                   join order in _context.Torders
                       on cartDetails.IIdCart equals order.IIdCart
                   where order.IIdCart == orderId && carpetInCart.BDeleted != true
                   select cartDetails;
        }

        #endregion
    }
}