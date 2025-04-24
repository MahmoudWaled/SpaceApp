using Microsoft.AspNetCore.Identity;
using Space.Domain.Identity;

namespace Space.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IApplicationUser> FindByEmailAsync(string email);

        Task<IApplicationUser> FindByIdAsync(string userId);

        Task<IdentityResult> CreateAsync(IApplicationUser user, string password);

        Task<IdentityResult> UpdateAsync(IApplicationUser user);

        Task<IdentityResult> DeleteAsync(string userId);

        Task<bool> CheckPasswordAsync(IApplicationUser user, string password);

        Task<string> GeneratePasswordResetTokenAsync(IApplicationUser user);

        Task<IdentityResult> ResetPasswordAsync(IApplicationUser user, string token, string newPassword);
    }
}