using AutoMapper;
using Pickpong.BL.Interfaces;
using Pickpong.DAL.Interfaces;
using Pickpong.Dto.Classes;
using Pickpong.Entities;
using Pickpong.Models;
using UpdateCarpetCountRequest = Pickpong.Dto.Classes.UpdateCarpetCountRequest;

namespace Pickpong.BL.Classes
{
    public enum DecreaseResult
    {
        Success,
        NotFound,
        CannotDecrease
    }

    public class OrderServiceBL : IOrderServiceBL
    {
        private readonly IOrderDL _orderDL;
        private readonly ICarpetDL _carpetDL;
        private readonly ICustomizesBL _customizesBL;
        private readonly IMapper _mapper;

        public OrderServiceBL(ICarpetDL carpetDL,
            ICustomizesBL customizesBL,
            IMapper mapper,
            IOrderDL orderDL)
        {
            _carpetDL = carpetDL;
            _customizesBL = customizesBL;
            _mapper = mapper;
            _orderDL = orderDL;
        }

        public async Task<List<CartDetails>> GetOrderDetailsWithPricesAsync(int orderId)
        {
            List<TcartDetail> carpetsInOrder = await _carpetDL.GetCarpetsInOrderAsync(orderId);
            List<CartDetails> cartDetails = _mapper.Map<List<CartDetails>>(carpetsInOrder);

            if (cartDetails == null || !cartDetails.Any())
                return new();

            List<TcartDetail> carpetsWithRegularPrices = await _carpetDL.GetCarpetsWithPricesAsync(orderId);
            List<CartDetails> mappedPrices = _mapper.Map<List<CartDetails>>(carpetsWithRegularPrices);

            MergePrices(cartDetails, mappedPrices);
            await CalculateMissingPricesAsync(cartDetails);

            return cartDetails;
        }

        public async Task<List<CartDetails>> GetOrdersDetailsAsync(int userId)
        {
            List<CartDetails> allOrders = await _orderDL.GetAllOrdersAsync(userId);
            List<CartDetails> regularPrices = await _orderDL.GetOrdersWithRegularPricesAsync(userId);

            MergePrices(allOrders, regularPrices);
            await CalculateMissingPricesAsync(allOrders);

            return allOrders;
        }

        public async Task<List<List<OrdersDetailModel>>> GetGroupedOrdersDetailsAsync(int userId)
        {
            List<OrdersDetailModel> ordersDetails = await _orderDL.GetOrdersDetailsAsync(userId)
                ?? new List<OrdersDetailModel>();
            return ordersDetails
                .GroupBy(o => o.IIdOrder)
                .Select(g => g.ToList())
                .ToList();
        }

        public async Task<List<OrderGroupModel>> GetGroupedOrdersDetailsAsync2(int userId)
        {
            return await _orderDL.GetGroupedOrdersDetailsAsync(userId);
        }


        public async Task<int> CreateNewOrderAsync(int userId, int carpetId)
        {
            Torder order = new Torder
            {
                DtOrderDate = DateTime.Now,
                IIdCart = await _orderDL.GetCartIdByCarpetAsync(userId, carpetId),
                //ISysTableRowIdShippingType,
                //ISysTableRowIdOrderStatus = 1,
                //IIdInvoice = 0,
                //DPrice,
                //ISysTableRowIdPaymentType = 9,
                //NvShippingNote,
            };

            return await _orderDL.AddOrderAsync(order);
        }

        public async Task<DecreaseResult> DecreaseCarpetCountAsync(int carpetId)
        {
            UpdateCountResult result = await UpdateCarpetCountAsync(new UpdateCarpetCountRequest
            {
                IIdCarpet = carpetId,
                CountChange = -1
            });

            return result switch
            {
                UpdateCountResult.Success => DecreaseResult.Success,
                UpdateCountResult.NotFound => DecreaseResult.NotFound,
                UpdateCountResult.InvalidCount => DecreaseResult.CannotDecrease,
                _ => DecreaseResult.CannotDecrease
            };
        }

        public async Task<UpdateCountResult> UpdateCarpetCountAsync(UpdateCarpetCountRequest request)
        {
            TcarpetDetailsInCart? carpet = await _orderDL.GetCarpetInCartAsync(request.IIdCarpet);
            if (carpet == null)
                return UpdateCountResult.NotFound;

            int newCount = carpet.ICount + request.CountChange;
            if (newCount < 1)
                return UpdateCountResult.InvalidCount;

            carpet.ICount = newCount;
            await _orderDL.SaveChangesAsync();
            return UpdateCountResult.Success;
        }

        public async Task<OrderStatusUpdateResult> MarkOrderAsReceivedAsync(int orderId)
        {
            Torder? order = await _orderDL.GetOrderByIdAsync(orderId);
            if (order == null)
                return OrderStatusUpdateResult.NotFound;

            if (order.ISysTableRowIdOrderStatus != 2)
                return OrderStatusUpdateResult.InvalidState;

            order.ISysTableRowIdOrderStatus = 3;
            await _orderDL.SaveChangesAsync();
            return OrderStatusUpdateResult.Success;
        }

        public async Task<bool> OrderCarpetAgainAsync(int userId, int carpetId)
        {
            TcartDetail cart = await _orderDL.GetOpenCartForUserAsync(userId) ??
                await _orderDL.CreateCartForUserAsync(userId);

            TcarpetDetailsInCart carpetInCart = new TcarpetDetailsInCart
            {
                IIdCarpet = carpetId,
                IIdCart = cart.IIdCart,
                ICount = 1,
                BDeleted = false
            };

            await _orderDL.AddCarpetToCartAsync(carpetInCart);
            return true;
        }


        public async Task<int> SaveOrderAsync(newOrderModel order)
        {
            TcustomerDetail newCustomer = _mapper.Map<TcustomerDetail>(order);
            await _orderDL.AddCustomerAsync(newCustomer);

            Torder newOrder = _mapper.Map<Torder>(order);
            await _orderDL.AddOrderAsync(newOrder);

            TcartDetail? cartDetails = await _orderDL.GetCartDetailsAsync(order.IIdCart);
            if (cartDetails != null)
            {
                cartDetails.BStatusPaid = true;
                await _orderDL.SaveChangesAsync();
            }

            return newOrder.IIdOrder;
        }

        // ---------- PRIVATE HELPERS ----------


        private void MergePrices(List<CartDetails> baseList, List<CartDetails> pricesList)
        {
            foreach (CartDetails item in baseList)
            {
                CartDetails? match = pricesList.FirstOrDefault(p => p.IIdCarpet == item.IIdCarpet);
                if (match != null)
                    item.DPrice = match.DPrice;
            }
        }

        private async Task CalculateMissingPricesAsync(List<CartDetails> items)
        {
            foreach (CartDetails item in items.Where(i => i.DPrice == 0))
            {
                Result<decimal> result = await _customizesBL.CalculateCustomPriceAsync(item);
                if (result.Success)
                {
                    item.DPrice = result.Value;
                }
            }
        }
    }
}