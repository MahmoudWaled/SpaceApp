using Space.Domain.Entities;

namespace Space.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(string id);

        Task<IEnumerable<Comment>> GetByPostIdAsync(string postId);

        Task AddAsync(Comment comment);

        Task DeleteAsync(Comment comment);
    }
}