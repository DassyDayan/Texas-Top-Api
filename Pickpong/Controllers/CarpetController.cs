using Microsoft.AspNetCore.Mvc;
using Pickpong.BL.Carpet;
using Pickpong.BL.Interfaces;
using Pickpong.Dto.Classes;
using Pickpong.Entities;
using Pickpong.Login;
using Pickpong.Models;

namespace Pickpong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CarpetController : ControllerBase
    {

        private readonly ICarpetService _carpetService;
        private readonly ICustomizesBL _customizesBL;
        private readonly IOrderServiceBL _orderService;

        public CarpetController(ICarpetService carpetService,
            ICustomizesBL customizesBL,
            IOrderServiceBL orderService)
        {
            _carpetService = carpetService;
            _customizesBL = customizesBL;
            _orderService = orderService;
        }

        [HttpPost]
        [CustomAuthorize]
        public async Task<IActionResult> CreateCarpet([FromBody] carpetDetailModel model)//יוצר שטיח ועגלה
        //add validation chaeck before sending to service
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized("UserId missing from context.");

            try
            {
                Result<List<CartDetailsDto>> result = await _carpetService.CreateCarpetAsync(model, userId);
                //return all carpets in cart
                return result.Success ? Ok(result) : StatusCode(500, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
        }

        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            if (HttpContext.Items.TryGetValue("UserId", out object? userIdObj) && userIdObj is int id)
            {
                userId = id;
                return true;
            }
            return false;
        }


        [HttpGet("{carpetId}")]
        public async Task<ActionResult<CarpetDetailsResponse>> GetCarpetDetailsAsync(int carpetId)// V
        {
            if (carpetId <= 0)
                return BadRequest("Invalid carpet ID.");
            try
            {
                CarpetDetailsResponse result = await _carpetService.GetCarpetDetailsAsync(carpetId);
                if (result?.Players == null || result.CarpetDetails == null)
                    return NotFound("Carpet not found.");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error fetching carpet details.");
            }
        }


        [HttpPost("multiple")]
        public async Task<ActionResult<List<TcarpetDetail>>> GetMultipleCarpetDetailsAsync([FromBody] CarpetIdsRequest request)
        //return without players
        {
            if (request?.CarpetIds == null || request.CarpetIds.Count == 0)
            {
                return BadRequest("No carpet IDs provided.");
            }
            try
            {
                List<TcarpetDetail> carpets = await _carpetService.GetCarpetsByIdsAsync(request.CarpetIds);
                if (carpets == null || carpets.Count == 0)
                    return NotFound("No carpets found for the given IDs.");

                return Ok(carpets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error occurred: {ex.Message}");
            }
        }
    }
    public class CarpetIdsRequest
    {
        public List<int>? CarpetIds { get; set; }
    }
}