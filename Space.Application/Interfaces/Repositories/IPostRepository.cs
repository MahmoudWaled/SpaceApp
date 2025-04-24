using Space.Domain.Entities;

namespace Space.Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetByIdAsync(string id);

        Task<IEnumerable<Post>> GetAllAsync();

        Task AddAsync(Post post);

        Task UpdateAsync(Post post);

        Task DeleteAsync(Post post);

        Task SaveChangesAsync();

        Task<IEnumerable<Post>> GetByUserIdAsync(string userId);
    }
}