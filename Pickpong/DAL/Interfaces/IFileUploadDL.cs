namespace Pickpong.DAL.Interfaces
{
    public interface IFileUploadDL
    {
        Task SaveFileAsync(IFormFile file, string path);
        Task ConvertPdfToBmpThumbnailsAsync(string pdfPath, string outputFolder, string baseFileName);
    }
}