using Microsoft.AspNetCore.Http;

namespace Recruitment.Application.Interfaces.Services.File
{
    public interface IFileStorageService
    {
        Task<string> SaveCVAsync(IFormFile file);
        void DeleteFile(string relativePath);
        bool IsValidCV(IFormFile file);
    }
}
