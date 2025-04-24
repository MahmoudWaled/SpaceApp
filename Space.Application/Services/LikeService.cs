using AutoMapper;
using Space.Application.DTOs.Likes;
using Space.Application.DTOs.Notifications;
using Space.Application.Interfaces.Repositories;
using Space.Application.Interfaces.Services;
using Space.Domain.Entities;

namespace Space.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository likeRepository;
        private readonly IMapper mapper;
        private readonly IPostRepository postRepository;
        private readonly INotificationService notificationService;

        public LikeService(ILikeRepository likeRepository, IMapper mapper, IPostRepository postRepository, INotificationService notificationService)
        {
            this.likeRepository = likeRepository;
            this.mapper = mapper;
            this.postRepository = postRepository;
            this.notificationService = notificationService;
        }

        public async Task<int> CountLikesOnPostAsync(string postId)
        {
            if (string.IsNullOrEmpty(postId))
            {
                throw new NullReferenceException("Post ID cannot be null or empty.");
            }
            var post = await postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post is not found.");
            }

            return await likeRepository.CountLikesOnPostAsync(postId);
        }

        public async Task LikePost(LikeDto dto)
        {
            if (dto == null)
            {
                throw new NullReferenceException("like dto cannot be null");
            }
            var post = await postRepository.GetByIdAsync(dto.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post you want to like is not found.");
            }
            var existingLike = await likeRepository.HasUserLikedPostAsync(dto.UserId, dto.PostId);
            if (existingLike)
            {
                throw new InvalidOperationException("You have already liked this post.");
            }

            await likeRepository.AddAsync(mapper.Map<Like>(dto));
            var newLike = await likeRepository.GetLikeAsync(dto.UserId, dto.PostId);
            if (post.UserId != dto.UserId)
            {
                await notificationService.CreateNotificationAsync(
                    newLike.UserId,
                    new CreateNotificationDto
                    {
                        UserId = post.UserId,
                        ActorId = newLike.UserId,
                        Type = "Like",
                        EntityId = newLike.Id,
                        Title = $"{newLike.User.UserName} liked your post ( {post.TextContent.Substring(0, post.TextContent.Length > 20 ? 20 : post.TextContent.Length) + "..."}) ",
                    }

                );
            }
        }

        public async Task UnlikePost(LikeDto dto)
        {
            if (dto == null)
            {
                throw new NullReferenceException("like dto cannot be null");
            }
            var post = await postRepository.GetByIdAsync(dto.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post you want to unlike is not found.");
            }
            var existingLike = await likeRepository.HasUserLikedPostAsync(dto.UserId, dto.PostId);
            if (!existingLike)
            {
                throw new InvalidOperationException("You have not liked this post yet.");
            }
            var like = await likeRepository.GetLikeAsync(dto.UserId, dto.PostId);

            if (like == null)
                throw new KeyNotFoundException("Like not found.");

            await likeRepository.RemoveAsync(like);
        }
    }
}