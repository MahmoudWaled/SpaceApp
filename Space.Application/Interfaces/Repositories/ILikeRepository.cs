using Space.Domain.Entities;

namespace Space.Application.Interfaces.Repositories
{
    public interface ILikeRepository
    {
        Task<bool> HasUserLikedPostAsync(string userId, string postId);

        Task AddAsync(Like like);

        Task RemoveAsync(Like like);

        Task<int> CountLikesOnPostAsync(string postId);

        Task<Like?> GetLikeAsync(string userId, string postId);
    }
}