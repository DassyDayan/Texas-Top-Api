using Microsoft.EntityFrameworkCore;
using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Classes
{
    public class CartDL : ICartDL
    {
        private readonly TexasTopContext _context;

        public CartDL(TexasTopContext context)
        {
            _context = context;
        }

        public async Task<TcartDetail?> GetUserCartAsync(int userId)
        {
            return await _context.TcartDetails
                .FirstOrDefaultAsync(c => c.IIdCustomer == userId && !c.BStatusPaid);
        }

        public async Task<TcartDetail> CreateCartAsync(int userId)
        {
            TcartDetail newCart = new TcartDetail
            {
                IIdCustomer = userId,
                BStatusPaid = false
            };

            await _context.TcartDetails.AddAsync(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }

        public async Task AddCarpetToCartAsync(TcarpetDetailsInCart carpetInCart)
        {
            await _context.TcarpetDetailsInCarts.AddAsync(carpetInCart);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TcartDetail>> GetCartsWithCarpetsAsync(int userId)
        {
            return await _context.TcartDetails
                .Include(c => c.TcarpetDetailsInCarts)
                .Where(c => c.IIdCustomer == userId && !c.BStatusPaid)
                .ToListAsync();
        }

        public async Task<Dictionary<int, TcarpetDetail>> GetCarpetDetailsMapAsync(List<int> carpetIds)
        {
            return await _context.TcarpetDetails
                .Where(cd => carpetIds.Contains(cd.IIdCarpet))
                .ToDictionaryAsync(cd => cd.IIdCarpet);
        }

        public async Task<List<CartDetails>> GetCarpetsInCurrentCartAsync(int userId)//לבדוק שוב איך עובד
                                                                                     //רשימת כל השטיחים שנמצאים בעגלה הלא-משולמת של המשתמש.
        {
            return await (
                from cart in _context.TcartDetails
                join carpetInCart in _context.TcarpetDetailsInCarts on cart.IIdCart equals carpetInCart.IIdCart
                join carpet in _context.TcarpetDetails on carpetInCart.IIdCarpet equals carpet.IIdCarpet
                where cart.IIdCustomer == userId && !cart.BStatusPaid && !carpetInCart.BDeleted
                select new CartDetails
                {
                    IIdCarpet = carpet.IIdCarpet,
                    IIdShape = carpet.IIdShape,
                    IIdColor = carpet.IIdColor,
                    FSizeParameterA = carpet.FSizeParameterA,
                    FSizeParameterB = carpet.FSizeParameterB,
                    DPrice = 0,
                    ICount = carpetInCart.ICount,
                    NvCarpetFileName = carpet.NvCarpetFileName,
                    NvSmallCarpetFileName = carpet.NvSmallCarpetFileName
                }).ToListAsync();
        }

        public async Task<Dictionary<int, decimal>> GetRegularCarpetPricesAsync(int userId)
        {
            List<CarpetPriceInfo> result = await (
                from cart in _context.TcartDetails
                join carpetInCart in _context.TcarpetDetailsInCarts on cart.IIdCart equals carpetInCart.IIdCart
                join carpet in _context.TcarpetDetails on carpetInCart.IIdCarpet equals carpet.IIdCarpet
                join setting in _context.TboardSettings on new
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
                where cart.IIdCustomer == userId && !carpetInCart.BDeleted
                select new CarpetPriceInfo
                {
                    CarpetId = carpet.IIdCarpet,
                    Price = setting.DPrice
                }).ToListAsync();

            //return result.ToDictionary(x => x.CarpetId, x => x.Price);

            return result
                .GroupBy(x => x.CarpetId)
                .ToDictionary(g => g.Key, g => g.Last().Price);
        }

        public async Task<decimal> CalculateCustomPriceAsync(CartDetails carpet)
        {
            return await (
                from custom in _context.Tcustomizes
                where carpet.IIdShape == custom.IIdShape
                select Convert.ToDecimal(custom.DPriceForMeter *
                    (Convert.ToDecimal(carpet.FSizeParameterA) / 100) *
                    (Convert.ToDecimal(carpet.FSizeParameterB) / 100))
            ).FirstOrDefaultAsync();
        }
    }

    public class CarpetPriceInfo
    {
        public int CarpetId { get; set; }
        public decimal Price { get; set; }
    }
}