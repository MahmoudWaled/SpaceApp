using Microsoft.AspNetCore.Http;

namespace Space.Application.DTOs.Users
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Bio { get; set; }
        public IFormFile? profileImagePath { get; set; }
    }
}