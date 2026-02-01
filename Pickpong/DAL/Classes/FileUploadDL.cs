using Aspose.Pdf;
using Aspose.Pdf.Devices;
using Pickpong.DAL.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;

namespace Pickpong.DAL.Classes
{
    public class FileUploadDL : IFileUploadDL
    {
        public async Task SaveFileAsync(IFormFile file, string path)
        {
            await using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(stream).ConfigureAwait(false);
        }

        public async Task ConvertPdfToBmpThumbnailsAsync(string pdfPath, string outputFolder, string baseFileName)
        {
            using Document pdfDocument = new Document(pdfPath);
            Resolution resolution = new Resolution(300);
            JpegDevice jpegDevice = new JpegDevice(resolution);

            for (int pageNumber = 1; pageNumber <= pdfDocument.Pages.Count; pageNumber++)
            {
                using MemoryStream imageStream = new MemoryStream();

                jpegDevice.Process(pdfDocument.Pages[pageNumber], imageStream);
                imageStream.Seek(0, SeekOrigin.Begin);

                using Image<Rgba32> image = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(imageStream).ConfigureAwait(false);

                string fileName = $"{baseFileName}_{pageNumber}_{Guid.NewGuid():N}.bmp";
                string filePath = Path.Combine(outputFolder, fileName);

                await image.SaveAsync(filePath, new BmpEncoder()).ConfigureAwait(false);
            }
        }
    }
}