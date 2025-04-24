using Microsoft.AspNetCore.Http;
using Space.Application.Interfaces.Services;
using System.Security.Claims;

namespace Space.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string? GetUserName()
        {
            return httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        public string? GetUserEmail()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        }
    }
}