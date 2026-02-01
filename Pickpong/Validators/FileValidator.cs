using Pickpong.Models;


namespace Pickpong.Validators
{
    public static class FileValidator
    {

        public static async Task<Result> ValidatePdfAsync(IFormFile file)
        {
            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return Result.Failure("הקובץ חייב להיות מסוג PDF בלבד.");

            if (file.ContentType != "application/pdf")
                return Result.Failure("הקובץ חייב להיות מסוג PDF בלבד.");

            using (Stream stream = file.OpenReadStream())
            {
                byte[] buffer = new byte[5];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                string header = System.Text.Encoding.ASCII.GetString(buffer);
                if (!header.StartsWith("%PDF-"))
                    return Result.Failure("הקובץ אינו קובץ PDF תקין.");
            }

            return Result.SuccessResult("קובץ תקין.");
        }

        public static Result ValidateImage(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".svg" };
            string[] allowedContentTypes = { "image/jpeg", "image/jpg", "image/png", "image/svg+xml" };

            if (!allowedExtensions.Contains(extension))
                return Result.Failure("סוג הקובץ לא נתמך. ניתן להעלות קבצים מסוג PNG, JPG, JPEG, או SVG.");

            if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
                return Result.Failure("סוג תוכן הקובץ לא תקין.");

            return Result.SuccessResult("קובץ תמונה תקין.");
        }
    }
}