using Microsoft.AspNetCore.Identity;
using Space.Application.Interfaces.Repositories;
using Space.Domain.Identity;
using Space.Infrastructure.Data;

namespace Space.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<bool> CheckPasswordAsync(IApplicationUser user, string password)
        {
            if (user is not ApplicationUser appUser)
                throw new ArgumentException("User must be of type ApplicationUser");
            return await userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<IdentityResult> CreateAsync(IApplicationUser user, string password)
        {
            if (user is not ApplicationUser appUser)
                throw new ArgumentException("User must be of type ApplicationUser");

            var result = await userManager.CreateAsync(appUser, password);

            await context.SaveChangesAsync();
            return result;
        }

        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            var result = await userManager.DeleteAsync(user);
            return result;
        }

        public async Task<IApplicationUser> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IApplicationUser> FindByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> UpdateAsync(IApplicationUser user)
        {
            if (user is not ApplicationUser appUser)
                throw new ArgumentException("User must be of type ApplicationUser");

            var result = await userManager.UpdateAsync(appUser);
            return result;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(IApplicationUser user)
        {
            if (user is not ApplicationUser appUser)
                throw new ArgumentException("User must be of type ApplicationUser");

            return await userManager.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(IApplicationUser user, string token, string newPassword)
        {
            if (user is not ApplicationUser appUser)
                throw new ArgumentException("User must be of type ApplicationUser");

            var result = await userManager.ResetPasswordAsync(appUser, token, newPassword);
            return result;
        }
    }
}