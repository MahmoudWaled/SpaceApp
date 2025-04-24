using Space.Domain.Entities;
using Space.Domain.Identity;

namespace Space.Application.Interfaces.Repositories
{
    public interface IFollowRepository
    {
        Task<bool> IsFollowingAsync(string followerId, string followeeId);

        Task<IEnumerable<IApplicationUser>> GetFollowersAsync(string userId);

        Task<IEnumerable<IApplicationUser>> GetFollowingAsync(string userId);

        Task<bool> AddFollowAsync(Follow follow);

        Task<bool> RemoveFollowAsync(string followerId, string followeeId);
    }
}