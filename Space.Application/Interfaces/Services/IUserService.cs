using Space.Application.DTOs.Users;

namespace Space.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto dto);

        Task<string> LoginAsync(LoginUserDto dto);

        Task<UserDto> GetUserProfileAsync(string userId);

        Task<UserDto> UpdateUserProfileAsync(string userId, UpdateUserDto dto);

        Task<bool> DeleteUserAsync(string userId);

        Task RequestPasswordResetAsync(RequestResetPasswordDto dto);

        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}