using Space.Application.DTOs.Likes;

namespace Space.Application.Interfaces.Services
{
    public interface ILikeService
    {
        Task LikePost(LikeDto dto);

        Task UnlikePost(LikeDto dto);

        Task<int> CountLikesOnPostAsync(string postId);
    }
}