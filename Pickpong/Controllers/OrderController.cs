using Microsoft.AspNetCore.Mvc;
using Pickpong.BL.Classes;
using Pickpong.BL.Interfaces;
using Pickpong.Dto.Classes;
using Pickpong.Entities;
using Pickpong.Login;
using System.ComponentModel.DataAnnotations;
using UpdateCarpetCountRequest = Pickpong.Dto.Classes.UpdateCarpetCountRequest;

namespace Pickpong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderServiceBL _orderServiceBL;
        private readonly ICustomizesBL _customizesBL;

        public OrderController(
            IOrderServiceBL orderServiceBL,
            ICustomizesBL customizesBL)
        {
            _orderServiceBL = orderServiceBL;
            _customizesBL = customizesBL;
        }

        //היסטוריית הזמנות של משתמש
        [HttpGet("OrdersDetails")]
        [CustomAuthorize]
        public async Task<IActionResult> OrdersDetails()
        {
            if (!(HttpContext.Items["UserId"] is int userId) || userId == 0)
                return Unauthorized("UserId is missing or invalid.");
            try
            {
                List<List<OrdersDetailModel>> groupedOrders =
                    await _orderServiceBL.GetGroupedOrdersDetailsAsync(userId);
                return Ok(groupedOrders);
            }
            catch
            {
                return StatusCode(500, "Error fetching order details.");
            }
        }

        [HttpPost("OrderAgain")]
        [CustomAuthorize]
        public async Task<IActionResult> OrderAgain([FromBody] OrderAgainRequest request)//check again
        {
            if (!(HttpContext.Items["UserId"] is int userId) || userId == 0)
                return Unauthorized("UserId is missing or invalid.");

            if (request == null || request.IidCarpet <= 0)
                return BadRequest("Invalid request data.");

            try
            {
                bool success = await _orderServiceBL.OrderCarpetAgainAsync(userId, request.IidCarpet);

                if (!success)
                    return NotFound($"Carpet with ID {request.IidCarpet} could not be reordered.");

                return Ok(new { Message = "Order placed successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPatch("CustomerGotOrder")]//mark order as delivered by Id
        [CustomAuthorize]
        public async Task<IActionResult> CustomerGotOrder([FromBody] OrderStatusUpdateRequest request)
        {
            if (request == null || request.IIdOrder < 0)
            {
                return BadRequest("Invalid order ID.");
            }
            try
            {
                OrderStatusUpdateResult result = await _orderServiceBL.MarkOrderAsReceivedAsync(request.IIdOrder);
                return result switch
                {
                    OrderStatusUpdateResult.Success => Ok(),
                    OrderStatusUpdateResult.NotFound => NotFound("Order not found."),
                    OrderStatusUpdateResult.InvalidState =>
                    BadRequest("Order is not in a valid state to be marked as received."),
                    _ => StatusCode(500, "Unexpected error.")
                };
            }
            catch
            {
                return StatusCode(500, "Error updating order status.");
            }
        }


        [HttpPatch("UpdateCarpetCount")]//change carpet count
        [CustomAuthorize]
        public async Task<IActionResult> UpdateCarpetCount([FromBody] UpdateCarpetCountRequest request)
        {
            if (request == null || request.IIdCarpet <= 0)
            {
                return BadRequest("Invalid carpet ID or count change.");
            }
            try
            {
                UpdateCountResult result = await _orderServiceBL.UpdateCarpetCountAsync(request);
                return result switch
                {
                    UpdateCountResult.Success => Ok(),
                    UpdateCountResult.NotFound => NotFound("Carpet not found."),
                    UpdateCountResult.InvalidCount =>
                    BadRequest("Carpet count cannot be less than 1."),
                    _ => StatusCode(500, "Unexpected error.")
                };
            }
            catch
            {
                return StatusCode(500, "Error updating carpet count.");
            }
        }


        [HttpPatch("DecreaseCarpetCount")]//-1 fror carpet count by carpet Id
        [CustomAuthorize]
        public async Task<IActionResult> DecreaseCarpetCount([FromBody, Required] int? IIdCarpet)
        {
            if (IIdCarpet == null || IIdCarpet <= 0)
            {
                return BadRequest("Invalid carpet ID.");
            }
            try
            {
                DecreaseResult result = await _orderServiceBL.DecreaseCarpetCountAsync(IIdCarpet.Value);
                return result switch
                {
                    DecreaseResult.NotFound => NotFound("Carpet not found."),
                    DecreaseResult.CannotDecrease => BadRequest("Carpet count cannot be less than 1."),
                    DecreaseResult.Success => Ok(),
                    _ => StatusCode(500, "Unexpected error.")
                };
            }
            catch
            {
                return StatusCode(500, "Error decreasing carpet count.");
            }
        }


        [HttpPost("PaymentAndCreateNewOrder")]
        [CustomAuthorize]//rewrite the function again
        public async Task<IActionResult> PaymentAndCreateNewOrder([FromQuery, Required] int? IIdCarpet)
        {
            if (!(HttpContext.Items["UserId"] is int userId))
                return Unauthorized();

            if (IIdCarpet == null || IIdCarpet <= 0)
            {
                return BadRequest("Invalid carpet ID.");
            }
            try
            {
                int newOrderId = await _orderServiceBL.CreateNewOrderAsync(userId, IIdCarpet.Value);
                return Ok(new { OrderId = newOrderId });
            }
            catch
            {
                return StatusCode(500, "Error creating new order.");
            }
        }


        [HttpPost("OrdersDetails2")]//check again
        [CustomAuthorize]
        public async Task<IActionResult> OrdersDetails2()
        {
            if (!(HttpContext.Items["UserId"] is int userId))
                return Unauthorized();
            try
            {
                List<CartDetails> ordersDetails = await _orderServiceBL.GetOrdersDetailsAsync(userId);
                return Ok(ordersDetails);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error fetching orders details.");
            }
        }

        [HttpPost("SaveOrder")]
        [CustomAuthorize]
        public async Task<IActionResult> SaveOrder([FromBody] newOrderModel order)
        {
            if (!(HttpContext.Items["UserId"] is int userId))
                return Unauthorized();

            try
            {
                int orderId = await _orderServiceBL.SaveOrderAsync(order);
                return Ok(new { OrderId = orderId });
            }
            catch
            {
                return StatusCode(500, "An error occurred while saving the order.");
            }
        }


        [HttpGet("orders/{idOrder}")]
        public async Task<IActionResult> OrderDetails(int idOrder)
        {
            if (idOrder <= 0)
            {
                return BadRequest("Invalid or missing order ID.");
            }
            try
            {
                List<CartDetails> orderDetails = await _orderServiceBL.GetOrderDetailsWithPricesAsync(idOrder);
                if (orderDetails == null)
                {
                    return NotFound("order not found.");
                }
                return Ok(orderDetails);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error occurred: {ex.Message}");
            }
        }
    }
}