namespace Space.Application.DTOs.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}