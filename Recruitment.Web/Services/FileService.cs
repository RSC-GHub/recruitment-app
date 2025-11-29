using Recruitment.Application.Interfaces.Services.File;


namespace Recruitment.Web.Services
{
    public class FileService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private const string CVFolder = "uploads/cv";

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveCVAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            if (!IsValidCV(file))
                throw new InvalidOperationException("Only PDF or Word files are allowed");

            var folderPath = Path.Combine(_env.WebRootPath, CVFolder);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/" + Path.Combine(CVFolder, fileName).Replace("\\", "/");
        }

        public void DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
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
