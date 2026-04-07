using Recruitment.Application.Interfaces.Services.File;

namespace Recruitment.Web.Services
{
    public class FileService : IFileStorageService
    {
        private readonly string _cvPath;

        public FileService(IConfiguration configuration)
        {
            _cvPath = configuration["FileStorage:CvPath"]
                ?? throw new ArgumentNullException("FileStorage:CvPath is missing in configuration");
        }

        public async Task<string> SaveCVAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            if (!IsValidCV(file))
                throw new InvalidOperationException("Only PDF or Word files are allowed");

            if (!Directory.Exists(_cvPath))
                Directory.CreateDirectory(_cvPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_cvPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName; // store only filename in DB
        }

        public void DeleteFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            var fullPath = Path.Combine(_cvPath, fileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public bool IsValidCV(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return extension == ".pdf" || extension == ".docx" || extension == ".doc";
        }
    }
}