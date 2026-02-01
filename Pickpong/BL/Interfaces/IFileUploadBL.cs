using Pickpong.Models;

namespace Pickpong.BL.Interfaces
{
    public interface IFileUploadBL
    {
        //Task<Result> HandlePdfUploadAsync(IFormFile file, int userId);
        Task<Result> HandlePdfUploadAsync(IFormFile file, int userId, int carpetId);
        Task<Result> HandleLogoUploadAsync(IFormFile file);
    }
}