using Microsoft.AspNetCore.Http;

namespace Space.Application.DTOs.Posts
{
    public class UpdatePostDto
    {
        public string PostId { get; set; }

        public string? TextContent { get; set; }

        public IFormFile? ImagePath { get; set; }
    }
}