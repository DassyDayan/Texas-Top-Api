using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Pickpong.BL.Interfaces;
using Pickpong.DAL.Interfaces;
using Pickpong.Entities;
using Pickpong.Login;
using Pickpong.Models;
using Pickpong.Validators;

namespace Pickpong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TexasTopContext _context;
        private readonly ILoginBL _loginBL;
        private readonly IOrderServiceBL _orderServiceBL;
        private readonly ILoginDL _loginDL;

        public LoginController(IMemoryCache memoryCache, ILoginBL loginBL,
            IOrderServiceBL orderServiceBL, ILoginDL loginDL)
        {
            _memoryCache = memoryCache;
            _context = new TexasTopContext();
            _loginBL = loginBL;
            _orderServiceBL = orderServiceBL;
            _loginDL = loginDL;
        }


        [HttpPost("LoginManagment")]
        public IActionResult LoginManagment([FromBody] LoginModel userNameAndPass)
        {
            try
            {
                if (string.IsNullOrEmpty(userNameAndPass?.name) || string.IsNullOrEmpty(userNameAndPass?.password))
                    return BadRequest("Missing username or password");

                if ((userNameAndPass.name == "David_Pikpong" && userNameAndPass.password == "David@picpong.biz") ||
                    (userNameAndPass.name == "Kuki_Pikpong" && userNameAndPass.password == "Kuki@picpong.biz"))
                {
                    ResponseModel resp = new ResponseModel { iUserId = 0 };
                    resp.nvToken = _loginBL.GenerateToken(resp);
                    return Ok(resp);
                }
                return Unauthorized("Invalid credentials");
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] emailOrPhoneModel emailOrPhone)
        {
            if (string.IsNullOrWhiteSpace(emailOrPhone?.emailOrPhone))
                return BadRequest("Email or phone is required.");

            if (!ValidationUtils.IsValidEmailOrPhone(emailOrPhone.emailOrPhone))
                return BadRequest("Invalid email or phone format.");

            try
            {
                LoginParams loginParams = new LoginParams
                {
                    isMail = emailOrPhone.emailOrPhone.Contains("@"),
                    mobileOrMail = emailOrPhone.emailOrPhone
                };

                TcustomerDetail customer = await _loginDL.GetOrCreateCustomerAsync(loginParams);
                await _loginBL.SendOTPAsync(loginParams, customer);

                return Ok(new { customerId = customer.IIdCustomer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " (Login Function)");
            }
        }


        [HttpPost("AuthenticateOTP")]
        public async Task<IActionResult> AuthenticateOTP([FromBody] LoginParams? loginParams)
        {
            try
            {
                if (loginParams == null ||
                    string.IsNullOrWhiteSpace(loginParams.mobileOrMail) ||
                    string.IsNullOrWhiteSpace(loginParams.OTP))
                {
                    return BadRequest("Missing OTP or identifier.");
                }

                string key = loginParams.mobileOrMail.Trim();

                if (!ValidationUtils.IsValidEmailOrPhone(key))
                {
                    return BadRequest("Invalid phone number or email format.");
                }

                if (!_memoryCache.TryGetValue(key, out object? cached) ||
                    cached is not LoginParams cachedLoginParams)
                {
                    return NotFound("OTP expired or not requested.");
                }

                if (loginParams.OTP != cachedLoginParams.OTP)
                {
                    return Unauthorized("Invalid OTP code.");
                }

                TcustomerDetail? userModel = await _loginDL.GetCustomerByEmailOrPhoneAsync(key);

                if (userModel == null)
                {
                    return NotFound("User not found.");
                }

                ResponseModel res = new ResponseModel
                {
                    iUserId = userModel.IIdCustomer,
                    nvToken = _loginBL.GenerateToken(new ResponseModel
                    { iUserId = userModel.IIdCustomer })
                };

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost("LoginBy")]
        public async Task<IActionResult> LoginBy([FromBody] emailOrPhoneModel emailOrPhone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emailOrPhone?.emailOrPhone))
                    return BadRequest("Email or phone is required.");

                if (!ValidationUtils.IsValidEmailOrPhone(emailOrPhone.emailOrPhone))
                    return BadRequest("Invalid email or phone format.");

                TcustomerDetail? userModel = await _loginDL.GetCustomerByEmailOrPhoneAsync(emailOrPhone.emailOrPhone);
                // TcustomerDetail userModel = await _loginDL.GetOrCreateCustomerAsync(...);

                if (userModel == null)
                    return Unauthorized("User not found.");

                ResponseModel res = new ResponseModel
                {
                    iUserId = userModel.IIdCustomer,
                    nvToken = _loginBL.GenerateToken(new ResponseModel
                    { iUserId = userModel.IIdCustomer })
                };
                return Ok(res);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred in LoginBy.");
            }
        }


        [HttpPost("SendingCompletedOrder")]
        [CustomAuthorize]
        public async Task<IActionResult> SendCompletedOrder()
        {
            try
            {
                if (HttpContext.Items["UserId"] is not int userId)
                    return Unauthorized();

                TcustomerDetail? customer = await _loginDL.GetCustomerByIdAsync(userId);
                if (customer == null)
                    return NotFound("Customer not found.");

                int? latestOrderId = await _loginDL.GetLatestOrderIdByCustomerIdAsync(userId);

                if (latestOrderId == null || latestOrderId == 0)
                    return NotFound("No recent order found for customer.");

                string messageText = $"הזמנתך מספר {latestOrderId} התקבלה במערכת בהצלחה.";

                await _loginBL.NotifyCustomerOfCompletedOrderAsync(customer.NvEmailAddress, customer.NvPhoneNumber, messageText);

                return Ok(new
                {
                    latestOrderId,
                    customerEmail = customer.NvEmailAddress,
                    customerPhone = customer.NvPhoneNumber
                });
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred while sending confirmation.");
            }
        }
    }
}