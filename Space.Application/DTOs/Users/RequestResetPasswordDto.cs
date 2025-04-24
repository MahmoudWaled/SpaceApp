using System.ComponentModel.DataAnnotations;

namespace Space.Application.DTOs.Users
{
    public class RequestResetPasswordDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}