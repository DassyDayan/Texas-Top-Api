using Pickpong.DAL.Interfaces;
using Pickpong.Dto.Classes;
using Pickpong.Entities;
using Pickpong.Models;
using Pickpong.Services.Interfaces;

public class CartServiceBL : ICartServiceBL
{

    private readonly ICartDL _cartDL;

    public CartServiceBL(ICartDL cartDL)
    {
        _cartDL = cartDL;
    }


    public async Task AddCarpetToUserCartAsync(int userId, int carpetId)
    {
        TcartDetail cart = await _cartDL.GetUserCartAsync(userId) ??
            await _cartDL.CreateCartAsync(userId);

        TcarpetDetailsInCart carpetInCart = new TcarpetDetailsInCart
        {
            IIdCarpet = carpetId,
            IIdCart = cart.IIdCart,
            ICount = 1,
            BDeleted = false
        };

        await _cartDL.AddCarpetToCartAsync(carpetInCart);
    }

    public async Task<List<CartDetailsDto>> GetCartDetailsDtoAsync(int userId)
    {
        List<TcartDetail> carts = await _cartDL.GetCartsWithCarpetsAsync(userId);

        List<int> carpetIds = carts
            .SelectMany(c => c.TcarpetDetailsInCarts.Select(cc => cc.IIdCarpet))
            .Distinct()
            .ToList();

        Dictionary<int, TcarpetDetail> carpetDetailsMap =
            await _cartDL.GetCarpetDetailsMapAsync(carpetIds);

        return carts.Select(c => new CartDetailsDto
        {
            IIdCart = c.IIdCart,
            BStatusPaid = c.BStatusPaid,
            Carpets = c.TcarpetDetailsInCarts.Select(cc => new CarpetDetailDto
            {
                IIdCarpet = cc.IIdCarpet,
                ICount = cc.ICount,
                CarpetDetail = carpetDetailsMap.TryGetValue(cc.IIdCarpet, out TcarpetDetail? detail)
                    ? detail
                    : throw new Exception($"Missing carpet detail for ID {cc.IIdCarpet}")
            }).ToList()
        }).ToList();
    }

    public async Task<List<CartDetails>> GetCartDetailsAsync(int userId)
    {
        List<CartDetails> carpetsInCart = await _cartDL.GetCarpetsInCurrentCartAsync(userId);
        Dictionary<int, decimal> regularPrices = await _cartDL.GetRegularCarpetPricesAsync(userId);

        foreach (CartDetails carpet in carpetsInCart)
        {
            if (carpet.IIdCarpet.HasValue && regularPrices
                .TryGetValue(carpet.IIdCarpet.Value, out decimal price))
            {
                carpet.DPrice = price;
            }
        }

        foreach (CartDetails carpet in carpetsInCart.Where(c => c.DPrice == 0))
        {
            carpet.DPrice = await _cartDL.CalculateCustomPriceAsync(carpet);
        }

        return carpetsInCart;
    }
}