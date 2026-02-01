using Microsoft.AspNetCore.Mvc;
using Pickpong.BL.Interfaces;
using Pickpong.Login;
using Pickpong.Models;

[Route("api/[controller]")]
[ApiController]
public class FilesController : Controller
{

    private readonly IFileUploadBL _fileUploadBL;

    public FilesController(IFileUploadBL fileUploadBL)
    {
        _fileUploadBL = fileUploadBL;
    }

    [HttpPost("pdf")]
    [CustomAuthorize]
    public async Task<IActionResult> UploadPdfAsync(int carpetId)
    {
        if (!(HttpContext.Items["UserId"] is int userId) || userId == 0)
            return Unauthorized("UserId is missing or invalid.");

        try
        {
            if (Request.Form.Files.Count == 0)
                return BadRequest("לא נמצא קובץ להעלאה.");

            IFormFile file = Request.Form.Files[0];
            Result result = await _fileUploadBL.HandlePdfUploadAsync(file, userId, carpetId);

            return result.Success ? Ok(result.Message) : StatusCode(500, result.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[UploadPdf Error] {ex.Message}");
            return StatusCode(500, "אירעה שגיאה בלתי צפויה במהלך העלאת הקובץ.");
        }
    }

    [HttpPost("logo")]
    public async Task<IActionResult> UploadLogoAsync([FromForm] IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                Result failureResult = Result.Failure("לא נמצא קובץ להעלאה.");
                return BadRequest(failureResult);
            }

            Result result = await _fileUploadBL.HandleLogoUploadAsync(file);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[UploadLogo Error] {ex.Message}");
            Result errorResult = Result.Failure("אירעה שגיאה בלתי צפויה במהלך העלאת הלוגו.", null, ex);
            return StatusCode(500, errorResult);
        }
    }
}