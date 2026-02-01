using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pickpong.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionTestController : ControllerBase
    {
        private readonly TexasTopContext _context;

        public ConnectionTestController(TexasTopContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                bool canConnect = await _context.Database.CanConnectAsync();

                if (canConnect)
                {
                    var connection = _context.Database.GetService<Microsoft.EntityFrameworkCore.Storage.IRelationalConnection>();

                    return Ok(new
                    {
                        Status = "Success",
                        Message = "התחברות למסד הנתונים הצליחה",
                        //DatabaseName = connection.DbConnection.Database,
                        //ServerName = connection.DbConnection.DataSource
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Status = "Error",
                        Message = "לא ניתן להתחבר למסד הנתונים"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Error",
                    Message = $"שגיSystem.TypeInitializationException: 'The type initializer for 'Microsoft.Data.SqlClient.SqlConnection' threw an exception.'אה בהתחברות למסד הנתונים: {ex.Message}"
                });
            }
        }
    }
}