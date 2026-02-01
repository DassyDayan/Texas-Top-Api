using Microsoft.AspNetCore.Mvc;
using Pickpong.BL.Interfaces;
using Pickpong.Entities;
using System.ComponentModel.DataAnnotations;

namespace Pickpong.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardSettingsController : ControllerBase
    {

        private readonly IBoardSettingsBL _boardSettingsService;

        public BoardSettingsController(IBoardSettingsBL boardSettingsService)
        {
            _boardSettingsService = boardSettingsService;
        }


        [HttpGet("sizes")]
        public async Task<IActionResult> GetSizesByShapeIdAsync([FromQuery][Required, Range(1, 4)] int? idShape)
        {
            if (idShape == null)
                return BadRequest("Missing shape ID.");

            if (idShape < 1 || idShape > 4)
                BadRequest("Invalid shape ID. Must be between 1 and 4.");

            try
            {
                List<SizeOptionsDto>? result = await _boardSettingsService.GetSizesByShapeIdAsync(idShape.Value);

                if (result == null || result.Count == 0)
                {
                    return NotFound("No sizes found for the given shape ID.");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Argument error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }

        }
    }
}