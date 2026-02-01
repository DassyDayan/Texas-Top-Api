namespace Pickpong.Utility
{
    public static class FileHelper
    {
        public static string GetSafeFileName(string originalFileName, string? forcedExtension = null)
        {
            string name = Path.GetFileNameWithoutExtension(originalFileName);
            string safeName = string.Concat(name.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));

            return safeName + (forcedExtension ?? Path.GetExtension(originalFileName));
        }

        public static void EnsureFileExistsAndNotEmpty(string path)
        {
            if (!System.IO.File.Exists(path))
                throw new FileNotFoundException($"File not found at path {path}");

            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Length == 0)
                throw new Exception("File is empty");
        }

    }
}