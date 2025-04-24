using Microsoft.AspNetCore.Http;

namespace Space.Application.DTOs.Users
{
    public class UpdateUserDto
    {
        public string? Email { get; set; } = null;
        public string? UserName { get; set; } = null;
        public string? Password { get; set; }
        public string? Bio { get; set; }
        public IFormFile? profileImagePath { get; set; }
    }
}