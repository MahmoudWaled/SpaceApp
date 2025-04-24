using Space.Application.DTOs.Follows;

namespace Space.Application.Interfaces.Services
{
    public interface IFollowService
    {
        Task<bool> IsFollowingAsync(string followerId, string followeeId);

        Task<IEnumerable<FollowUserDto>> GetFollowersAsync(string userId);

        Task<IEnumerable<FollowUserDto>> GetFollowingAsync(string userId);

        Task<bool> FollowUserAsync(string followerId, string followeeId);

        Task<bool> UnfollowUserAsync(string followerId, string followeeId);
    }
}