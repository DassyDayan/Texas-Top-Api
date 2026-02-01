using Microsoft.AspNetCore.Mvc;
using Pickpong.BL.Interfaces;
using Pickpong.Entities;

namespace Pickpong.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomizesController : ControllerBase
    {

        private readonly ICustomizesBL _customizesBL;

        public CustomizesController(ICustomizesBL customizesBL)
        {
            _customizesBL = customizesBL;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomizeSettingDTO>>> GetCustomizeSizesByIdShape([FromQuery] int? idShape)
        {
            if (idShape == null)
                return BadRequest("Missing shape ID.");

            if (idShape < 1 || idShape > 4)
                return BadRequest("Shape ID must be between 1 and 4.");

            try
            {
                List<CustomizeSettingDTO>? customizeSizes = await _customizesBL.GetCustomizeSizesByShapeIdAsync(idShape.Value);

                if (customizeSizes == null)
                    return BadRequest("Invalid shape ID.");

                if (customizeSizes.Count == 0)
                    return NotFound("No customize sizes found for given shape ID.");

                return Ok(customizeSizes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving customize sizes: {ex.Message}");
            }
        }
    }
}