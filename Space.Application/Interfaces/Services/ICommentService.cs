using Space.Application.DTOs.Comments;

namespace Space.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task AddCommentAsync(string userId, CreateCommentDto createCommentDto);

        Task DeleteCommentAsync(string userId, string commentId);

        Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId);

        Task<CommentDto> GetCommentByIdAsync(string commentId);
    }
}