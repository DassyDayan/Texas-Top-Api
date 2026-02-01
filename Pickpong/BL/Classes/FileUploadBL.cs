using Pickpong.BL.Interfaces;
using Pickpong.DAL.Interfaces;
using Pickpong.Models;
using Pickpong.Utility;
using Pickpong.Validators;

namespace Pickpong.BL.Classes
{
    public class FileUploadBL : IFileUploadBL
    {
        private readonly IFileUploadDL _fileUploadDL;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUploadBL(IFileUploadDL fileUploadDL, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _fileUploadDL = fileUploadDL;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> HandlePdfUploadAsync(IFormFile file, int userId, int carpetId)
        {
            try
            {
                Result validationResult = await FileValidator.ValidatePdfAsync(file);
                if (!validationResult.Success)
                    return validationResult;

                //string uploadDir = Path.Combine(webRoot, "Logos");

                string uploadDir = Path.Combine(_env.WebRootPath, "images", $"CarpetNo_{carpetId}");

                string fileName = FileHelper.GetSafeFileName(file.FileName, ".pdf");
                string filePath = Path.Combine(uploadDir, fileName);

                Directory.CreateDirectory(uploadDir);
                await _fileUploadDL.SaveFileAsync(file, filePath);

                FileHelper.EnsureFileExistsAndNotEmpty(filePath);

                await _fileUploadDL.ConvertPdfToBmpThumbnailsAsync(filePath, uploadDir,
                    Path.GetFileNameWithoutExtension(fileName));

                return Result.SuccessResult("הקובץ הועלה בהצלחה והומר לתמונות.");
            }
            catch (Exception ex)
            {
                return Result.Failure("שגיאה בהעלאת הקובץ.", "UPLOAD_FAILED", ex);
            }
        }

        //public async Task<Result> HandleLogoUploadAsync(IFormFile file)
        //{
        //    try
        //    {
        //        // בדיקת הקובץ
        //        Result validationResult = FileValidator.ValidateImage(file);
        //        if (!validationResult.Success)
        //            return validationResult;

        //        const long maxFileSize = 300 * 1024; // 300KB
        //        if (file.Length > maxFileSize)
        //            return Result.Failure("גודל הקובץ גדול מדי. אנא העלה קובץ עד 300KB.");

        //        // קביעת נתיב תיקיית העלאה במערכת הקבצים
        //        string webRoot = string.IsNullOrWhiteSpace(_env.WebRootPath)
        //            ? Path.Combine(_env.ContentRootPath, "wwwroot")
        //            : _env.WebRootPath;

        //        string uploadDir = Path.Combine(_env.WebRootPath, "Logos");
        //        Directory.CreateDirectory(uploadDir);

        //        // יצירת שם קובץ בטוח
        //        string fileName = FileHelper.GetSafeFileName(file.FileName);
        //        string filePath = Path.Combine(uploadDir, fileName);

        //        // שמירת הקובץ
        //        await _fileUploadDL.SaveFileAsync(file, filePath);

        //        // יצירת URL קבוע לגישה מהדפדפן
        //        string fileUrl = $"https://texastop.webit.systems/Logos/{fileName}";

        //        return Result.SuccessResult("לוגו הועלה בהצלחה." + fileUrl + " " + webRoot.ToString() + " " + uploadDir);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result.Failure($"שגיאה בהעלאת הלוגו: {ex.Message}");
        //    }
        //}


        public async Task<Result> HandleLogoUploadAsync(IFormFile file)
        {
            try
            {
                // 1️⃣ בדיקת תקינות קובץ
                Result validationResult = FileValidator.ValidateImage(file);
                if (!validationResult.Success)
                    return validationResult;

                // 2️⃣ בדיקת גודל מקסימלי (300KB)
                const long maxFileSize = 300 * 1024;
                if (file.Length > maxFileSize)
                    return Result.Failure("גודל הקובץ גדול מדי. יש להעלות קובץ עד 300KB.");

                // 3️⃣ קביעת תיקיית היעד
                string webRoot = string.IsNullOrWhiteSpace(_env.WebRootPath)
                    ? Path.Combine(_env.ContentRootPath, "wwwroot")
                    : _env.WebRootPath;

                string uploadDir = Path.Combine(webRoot, "Logos");
                Directory.CreateDirectory(uploadDir);

                //string uploadDir = Path.Combine(@"D:\Sites\Live\TexasTop", "Logos");
                //Directory.CreateDirectory(uploadDir);

                // 4️⃣ יצירת שם קובץ ייחודי ובטוח
                string safeFileName = FileHelper.GetSafeFileName(file.FileName);
                string uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";
                string fullPath = Path.Combine(uploadDir, uniqueFileName);

                // 5️⃣ שמירת הקובץ פיזית
                await _fileUploadDL.SaveFileAsync(file, fullPath);

                // 6️⃣ בניית כתובת URL לגישה לדפדפן
                string baseUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}";
                string relativeUrl = $"/Logos/{uniqueFileName}";
                string fileUrl = $"{baseUrl}{relativeUrl}";

                // 7️⃣ תשובת הצלחה
                return Result.SuccessResult($"לוגו הועלה בהצלחה: {fileUrl}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"שגיאה בהעלאת הלוגו: {ex.Message}");
            }
        }

    }
}