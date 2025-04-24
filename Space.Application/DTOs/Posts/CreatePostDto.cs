using Microsoft.AspNetCore.Http;

namespace Space.Application.DTOs.Posts
{
    public class CreatePostDto
    {
        public string? TextContent { get; set; }

        public IFormFile? ImagePath { get; set; }
    }
}