using Microsoft.AspNetCore.Http;

namespace Space.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
    }
}