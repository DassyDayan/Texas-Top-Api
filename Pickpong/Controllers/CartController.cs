using Microsoft.AspNetCore.Mvc;
using Pickpong.Entities;
using Pickpong.Login;
using Pickpong.Services.Interfaces;

namespace Pickpong.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {

        private readonly ICartServiceBL _cartBL;

        public CartController(ICartServiceBL cartBL)
        {
            _cartBL = cartBL;
        }

        [HttpGet]
        [CustomAuthorize]
        public async Task<IActionResult> CartDetails()// Get cart details for the authenticated user - not all worked out yet
        {
            if (!(HttpContext.Items["UserId"] is int userId) || userId == 0)
                return Unauthorized("UserId is missing or invalid.");

            try
            {
                List<CartDetails> cartDetails = await _cartBL.GetCartDetailsAsync(userId);
                return Ok(cartDetails);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving cart details.");
            }
        }
    }
}