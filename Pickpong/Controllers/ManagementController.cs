using Microsoft.AspNetCore.Mvc;
using Pickpong.BL.Interfaces;
using Pickpong.Entities;
using Pickpong.Login;
using Pickpong.Models;
using System.ComponentModel.DataAnnotations;

[Route("api/[controller]")]
[ApiController]
public class ManagementController : ControllerBase
{
    private readonly IManagementBL _managementBL;

    public ManagementController(IManagementBL managementBL)
    {
        _managementBL = managementBL;
    }


    [HttpGet("orders")]
    public async Task<ActionResult<List<List<OrdersDetailModel>>>> GetAllOrdersAsync()
    {
        try
        {
            List<List<OrdersDetailModel>> result = await _managementBL.GetAllOrdersAsync();
            if (result == null || result.Count == 0)
            {
                return Ok(new List<List<OrdersDetailModel>>());
            }
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error fetching all orders");
        }
    }


    [HttpGet("settings")]
    public async Task<ActionResult<List<TboardSetting>>> GetSettingsAsync([FromQuery, Range(1, 4), Required] int? shapeId)
    {
        if (shapeId == null)
        {
            return BadRequest("Shape ID is required.");
        }
        try
        {
            List<TboardSetting>? result = await _managementBL.GetSettingsAsync(shapeId.Value);
            if (result == null)
                return BadRequest("Invalid shape ID.");
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error fetching settings");
        }
    }


    [HttpGet("customize-settings")]
    public async Task<ActionResult<List<Tcustomize>>> GetCustomizeSettingsAsync([FromQuery, Range(1, 4), Required] int? shapeId)
    {
        if (shapeId == null)
        {
            return BadRequest("Shape ID is required.");
        }
        try
        {
            List<Tcustomize>? result = await _managementBL.GetCustomizeSettingsAsync(shapeId.Value);
            if (result == null)
                return BadRequest("Invalid shape ID.");
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error fetching customize settings");
        }
    }


    [HttpPut("settings/{id}")]
    public async Task<IActionResult> UpdateSettingAsync([Required] int? id, [FromBody] Settings setting)
    {
        if (id == null || setting.IIdShape > 4 || setting.IIdShape < 1)
        {
            return BadRequest("Invalid shape ID. It must be between 1 and 4.");
        }
        try
        {
            bool success = await _managementBL.UpdateSettingAsync(setting);
            return success ? Ok("Setting updated successfully:)") : NotFound("Setting not found or Invalid shape ID");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error updating setting");
        }
    }


    [HttpPost("settings")]
    public async Task<IActionResult> AddSettingAsync([FromBody] Settings setting)//להוסיף בדיקת תקינות
    {
        if (setting == null || setting.IIdShape < 1 || setting.IIdShape > 4)
        {
            return BadRequest("Invalid shape ID. It must be between 1 and 4.");
        }
        try
        {
            int result = await _managementBL.AddSettingAsync(setting);
            return result != -1
                ? Ok(new { Message = "Setting added successfully", Id = result })
                : BadRequest("Invalid shape ID");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error adding setting");
        }
    }


    [HttpPut("customize-settings/{id}")]
    public async Task<IActionResult> UpdateCustomizeSettingAsync([Required] int? id,
        [FromBody] SettingOfCustomize customize)//להוסיף בדיקת תקינות וכן ניתן להסיר את id
    {
        if (id == null || id < 0 || customize.IIdShape < 1 || customize.IIdShape > 4)
        {
            return BadRequest("Invalid ID or shape ID not valid.");
        }
        try
        {
            bool success = await _managementBL.UpdateCustomizeSettingAsync(customize);
            return success ? Ok($"Customize with shape ID {customize.IIdShape} updated successfully:)")
                : NotFound("Customize setting not found or Invalid shape ID");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error updating customize setting");
        }
    }


    [HttpPost("orders/{orderId}/mark-as-sent")]
    [CustomAuthorize]
    public async Task<IActionResult> MarkOrderAsSentAsync([Required] int orderId)
    //להוסיף בדיקת תקינות לבדוק אם מקבל orderid or OrderNumber
    {
        if (orderId <= 0)
        {
            return BadRequest("Invalid order ID.");
        }
        try
        {
            bool success = await _managementBL.MarkOrderAsSentAsync(orderId);
            return success ?
                Ok(new { message = "Order marked as send successfully :)" }) :
                NotFound(new { message = "Order not found" });
        }
        catch (Exception)
        {
            return StatusCode(500, "Error updating order status");
        }
    }


    [HttpDelete("settings/{id}")]
    public async Task<IActionResult> RemoveSettingAsync([Required] int? id)//להוסיף בדיקת תקינות
    {
        if (id == null || id < 0)
        {
            return BadRequest("Invalid ID.");
        }
        try
        {
            bool success = await _managementBL.RemoveSettingAsync(id.Value);
            return success ?
                Ok($"Settings with id{id} removed successfully:)") :
                NotFound("Setting not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "Error removing setting");
        }
    }
}