using Space.Application.DTOs.Posts;

namespace Space.Application.Interfaces.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();

        Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(string userId);

        Task<PostDto?> GetPostByIdAsync(string postId);

        Task CreatePostAsync(CreatePostDto dto);

        Task UpdatePostAsync(UpdatePostDto dto);

        Task DeletePostAsync(string postId);
    }
}