using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Space.Application.DTOs.Users;
using Space.Application.Interfaces.Services;

namespace Space.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ICurrentUserService currentUserService;

        public UsersController(IUserService userService, ICurrentUserService currentUserService)
        {
            this.userService = userService;
            this.currentUserService = currentUserService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
        {
            var userProfile = await userService.RegisterAsync(dto);
            return Ok(userProfile);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var token = await userService.LoginAsync(dto);
            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = currentUserService.GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated.");
            var profile = await userService.GetUserProfileAsync(userId);
            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile/{userId}")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserDto dto)
        {
            var userId = currentUserService.GetUserId();

            var updatedProfile = await userService.UpdateUserProfileAsync(userId, dto);
            return Ok(updatedProfile);
        }

        [Authorize]
        [HttpDelete("profile/{userId}")]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = currentUserService.GetUserId();
            await userService.DeleteUserAsync(userId);
            return NoContent();
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestResetPasswordDto dto)
        {
            await userService.RequestPasswordResetAsync(dto);
            return Ok(new { Message = "Password reset email sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await userService.ResetPasswordAsync(dto);
            return Ok(new { Message = "Password reset successfully." });
        }
    }
}