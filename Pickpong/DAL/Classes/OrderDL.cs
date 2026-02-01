using Microsoft.EntityFrameworkCore;
using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Models;

namespace Pickpong.DAL.Classes
{
    public class OrderDL : IOrderDL
    {
        private readonly TexasTopContext _context;

        public OrderDL(TexasTopContext context)
        {
            _context = context;
        }

        public async Task<List<OrdersDetailModel>> GetOrdersDetailsAsync(int userId)
        {
            return await (
                from customer in _context.TcustomerDetails
                join cart in _context.TcartDetails on customer.IIdCustomer equals cart.IIdCustomer
                join carpetInCart in _context.TcarpetDetailsInCarts on cart.IIdCart equals carpetInCart.IIdCart
                join order in _context.Torders on cart.IIdCart equals order.IIdCart
                join invoiceDetail in _context.TinvoiceDetails on order.IIdInvoice equals invoiceDetail.IIdInvoice
                where customer.IIdCustomer == userId && !carpetInCart.BDeleted
                select new OrdersDetailModel
                {
                    IidCarpet = carpetInCart.IIdCarpet,
                    ICount = carpetInCart.ICount,
                    IIdOrder = order.IIdOrder,
                    NvOrderNumber = order.NvOrderNumber,
                    DtOrderDate = order.DtOrderDate,
                    ISysTableRowIdShippingType = order.ISysTableRowIdShippingType,
                    ISysTableRowIdOrderStatus = order.ISysTableRowIdOrderStatus,
                    DPrice = order.DPrice,
                    NvFirstName = customer.NvFirstName,
                    NvLastName = customer.NvLastName,
                    NvPhoneNumber = customer.NvPhoneNumber,
                    ISysTableRowIdCity = customer.ISysTableRowIdCity,
                    ISysTableRowIdStreet = customer.ISysTableRowIdStreet,
                    IBuildingNumber = customer.IBuildingNumber,
                    IDepartmentNumber = customer.IDepartmentNumber,
                    NvZipCode = customer.NvZipCode,
                    NvDealerName = invoiceDetail.NvDealerName,
                    NvDealerNumber = invoiceDetail.NvDealerNumber,
                    DISysTableRowIdCity = invoiceDetail.ISysTableRowIdCity,
                    DISysTableRowIdStreet = invoiceDetail.ISysTableRowIdStreet,
                    DIBuildingNumber = invoiceDetail.IBuildingNumber,
                    DIDepartmentNumber = invoiceDetail.IDepartmentNumber,
                    DNvZipCode = invoiceDetail.NvZipCode
                }
            ).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<OrderGroupModel>> GetGroupedOrdersDetailsAsync(int userId)
        {
            var rawData = await (
                from customer in _context.TcustomerDetails
                join cart in _context.TcartDetails on customer.IIdCustomer equals cart.IIdCustomer
                join carpetInCart in _context.TcarpetDetailsInCarts on cart.IIdCart equals carpetInCart.IIdCart
                join order in _context.Torders on cart.IIdCart equals order.IIdCart
                join invoiceDetail in _context.TinvoiceDetails on order.IIdInvoice equals invoiceDetail.IIdInvoice
                where customer.IIdCustomer == userId && !carpetInCart.BDeleted
                select new
                {
                    order.IIdOrder,
                    order.NvOrderNumber,
                    order.DtOrderDate,
                    Detail = new OrdersDetailModel
                    {
                        IidCarpet = carpetInCart.IIdCarpet,
                        ICount = carpetInCart.ICount,
                        IIdOrder = order.IIdOrder,
                        NvOrderNumber = order.NvOrderNumber,
                        DtOrderDate = order.DtOrderDate,
                        ISysTableRowIdShippingType = order.ISysTableRowIdShippingType,
                        ISysTableRowIdOrderStatus = order.ISysTableRowIdOrderStatus,
                        DPrice = order.DPrice,
                        NvFirstName = customer.NvFirstName,
                        NvLastName = customer.NvLastName,
                        NvPhoneNumber = customer.NvPhoneNumber,
                        ISysTableRowIdCity = customer.ISysTableRowIdCity,
                        ISysTableRowIdStreet = customer.ISysTableRowIdStreet,
                        IBuildingNumber = customer.IBuildingNumber,
                        IDepartmentNumber = customer.IDepartmentNumber,
                        NvZipCode = customer.NvZipCode,
                        NvDealerName = invoiceDetail.NvDealerName,
                        NvDealerNumber = invoiceDetail.NvDealerNumber,
                        DISysTableRowIdCity = invoiceDetail.ISysTableRowIdCity,
                        DISysTableRowIdStreet = invoiceDetail.ISysTableRowIdStreet,
                        DIBuildingNumber = invoiceDetail.IBuildingNumber,
                        DIDepartmentNumber = invoiceDetail.IDepartmentNumber,
                        DNvZipCode = invoiceDetail.NvZipCode
                    }
                }).ToListAsync().ConfigureAwait(false);

            return rawData
                .GroupBy(x => new { x.IIdOrder, x.NvOrderNumber, x.DtOrderDate })
                .Select(g => new OrderGroupModel
                {
                    OrderId = g.Key.IIdOrder,
                    NvOrderNumber = g.Key.NvOrderNumber,
                    DtOrderDate = g.Key.DtOrderDate,
                    Items = g.Select(x => x.Detail).ToList()
                }).ToList();
        }

        public async Task<List<CartDetails>> GetAllOrdersAsync(int userId)
        {
            return await (
                from cart in _context.TcartDetails
                join carpetInCart in _context.TcarpetDetailsInCarts on cart.IIdCart equals carpetInCart.IIdCart
                join carpet in _context.TcarpetDetails on carpetInCart.IIdCarpet equals carpet.IIdCarpet
                where cart.IIdCustomer == userId
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
                }
            ).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<CartDetails>> GetOrdersWithRegularPricesAsync(int userId)
        {
            return await (
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
                where cart.IIdCustomer == userId
                select new CartDetails
                {
                    IIdCarpet = carpet.IIdCarpet,
                    IIdShape = carpet.IIdShape,
                    IIdColor = carpet.IIdColor,
                    FSizeParameterA = carpet.FSizeParameterA,
                    FSizeParameterB = carpet.FSizeParameterB,
                    DPrice = setting.DPrice,
                    ICount = carpetInCart.ICount,
                    NvCarpetFileName = carpet.NvCarpetFileName,
                    NvSmallCarpetFileName = carpet.NvSmallCarpetFileName
                }
            ).ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> AddOrderAsync(Torder order)
        {
            _context.Torders.Add(order);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return order.IIdOrder;
        }

        public async Task<int> GetCartIdByCarpetAsync(int userId, int carpetId)
        {
            return await (
                from cart in _context.TcartDetails
                join carpetInCart in _context.TcarpetDetailsInCarts on cart.IIdCart equals carpetInCart.IIdCart
                where cart.IIdCustomer == userId && carpetInCart.IIdCarpet == carpetId
                select cart.IIdCart
            ).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<TcarpetDetailsInCart?> GetCarpetInCartAsync(int carpetId)
        {
            return await _context.TcarpetDetailsInCarts
                .FirstOrDefaultAsync(c => c.IIdCarpet == carpetId)
                .ConfigureAwait(false);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Torder?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Torders
                .FirstOrDefaultAsync(o => o.IIdOrder == orderId)
                .ConfigureAwait(false);
        }

        public async Task<TcartDetail?> GetOpenCartForUserAsync(int userId)
        {
            return await _context.TcartDetails
                .FirstOrDefaultAsync(c => c.IIdCustomer == userId && !c.BStatusPaid)
                .ConfigureAwait(false);
        }

        public async Task<TcartDetail> CreateCartForUserAsync(int userId)
        {
            TcartDetail newCart = new TcartDetail
            {
                IIdCustomer = userId,
                BStatusPaid = false
            };

            _context.TcartDetails.Add(newCart);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return newCart;
        }

        public async Task<TcustomerDetail> AddCustomerAsync(TcustomerDetail customer)
        {
            _context.TcustomerDetails.Add(customer);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return customer;
        }

        public async Task<TcartDetail?> GetCartDetailsAsync(int cartId)
        {
            return await _context.TcartDetails
                .FirstOrDefaultAsync(x => x.IIdCart == cartId)
                .ConfigureAwait(false);
        }

        public async Task AddCarpetToCartAsync(TcarpetDetailsInCart carpetInCart)
        {
            _context.TcarpetDetailsInCarts.Add(carpetInCart);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<List<OrdersDetailModel>> GetAllOrdersAsync()
        {
            return await (
                from customer in _context.TcustomerDetails
                join cart in _context.TcartDetails
                on customer.IIdCustomer equals cart.IIdCustomer
                join carpetInCart in _context.TcarpetDetailsInCarts
                on cart.IIdCart equals carpetInCart.IIdCart
                join order in _context.Torders
                on cart.IIdCart equals order.IIdCart
                join invoiceDetail in _context.TinvoiceDetails
                on order.IIdInvoice equals invoiceDetail.IIdInvoice
                where cart.BStatusPaid && !carpetInCart.BDeleted
                select new OrdersDetailModel
                {
                    IidCarpet = carpetInCart.IIdCarpet,
                    ICount = carpetInCart.ICount,
                    IIdOrder = order.IIdOrder,
                    NvOrderNumber = order.NvOrderNumber,
                    DtOrderDate = order.DtOrderDate,
                    ISysTableRowIdShippingType = order.ISysTableRowIdShippingType,
                    ISysTableRowIdOrderStatus = order.ISysTableRowIdOrderStatus,
                    DPrice = order.DPrice,
                    NvFirstName = customer.NvFirstName,
                    NvLastName = customer.NvLastName,
                    NvPhoneNumber = customer.NvPhoneNumber,
                    ISysTableRowIdCity = customer.ISysTableRowIdCity,
                    ISysTableRowIdStreet = customer.ISysTableRowIdStreet,
                    IBuildingNumber = customer.IBuildingNumber,
                    IDepartmentNumber = customer.IDepartmentNumber,
                    NvZipCode = customer.NvZipCode,
                    NvDealerName = invoiceDetail.NvDealerName,
                    NvDealerNumber = invoiceDetail.NvDealerNumber,
                    DISysTableRowIdCity = invoiceDetail.ISysTableRowIdCity,
                    DISysTableRowIdStreet = invoiceDetail.ISysTableRowIdStreet,
                    DIBuildingNumber = invoiceDetail.IBuildingNumber,
                    DIDepartmentNumber = invoiceDetail.IDepartmentNumber,
                    DNvZipCode = invoiceDetail.NvZipCode
                }
            ).ToListAsync().ConfigureAwait(false);
        }
    }
}