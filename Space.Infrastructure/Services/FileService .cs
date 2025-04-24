using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Space.Application.Interfaces.Services;

namespace Space.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment env;

        public FileService(IHostEnvironment env)
        {
            this.env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var uploadsPath = Path.Combine(env.ContentRootPath, folderName);
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{folderName}/{fileName}";
        }
    }
}