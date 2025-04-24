using AutoMapper;
using Space.Application.DTOs.Comments;
using Space.Application.DTOs.Notifications;
using Space.Application.Interfaces.Repositories;
using Space.Application.Interfaces.Services;
using Space.Domain.Entities;

namespace Space.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IMapper mapper;
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationService notificationService;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IPostRepository postRepository, IUserRepository userRepository, INotificationService notificationService)
        {
            this.commentRepository = commentRepository;
            this.mapper = mapper;
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.notificationService = notificationService;
        }

        public async Task AddCommentAsync(string userId, CreateCommentDto createCommentDto)
        {
            if (createCommentDto == null)
            {
                throw new NullReferenceException("Comment cannot be null");
            }
            if (string.IsNullOrEmpty(createCommentDto.PostId))
            {
                throw new NullReferenceException("Post id cannot be null or empity");
            }
            if (string.IsNullOrEmpty(createCommentDto.Content))
            {
                throw new NullReferenceException("Content cannot be null or empity");
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new NullReferenceException("User id cannot be null or empity");
            }
            var post = await postRepository.GetByIdAsync(createCommentDto.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            var comment = mapper.Map<Comment>(createCommentDto);
            comment.UserId = userId;
            await commentRepository.AddAsync(comment);

            if (post.UserId != userId)
            {
                await notificationService.CreateNotificationAsync(
                    userId,
                    new CreateNotificationDto
                    {
                        UserId = post.UserId,
                        ActorId = userId,
                        Type = "Comment",
                        EntityId = comment.Id,
                        Title = $"{user.UserName} commented on your post ( {post.TextContent.Substring(0, post.TextContent.Length > 20 ? 20 : post.TextContent.Length) + "..."}) ",
                    }

                );
            }
        }

        public async Task DeleteCommentAsync(string userId, string commentId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NullReferenceException("User id cannot be null or empity");
            }
            if (string.IsNullOrEmpty(commentId))
            {
                throw new NullReferenceException("Comment id cannot be null or empity");
            }
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var comment = await commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found");
            }
            if (comment.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this comment.");
            }
            await commentRepository.DeleteAsync(comment);
        }

        public async Task<CommentDto> GetCommentByIdAsync(string commentId)
        {
            if (string.IsNullOrEmpty(commentId))
            {
                throw new NullReferenceException("Comment id cannot be null or empity");
            }
            var comment = await commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found");
            }
            return mapper.Map<CommentDto>(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId)
        {
            if (string.IsNullOrEmpty(postId))
            {
                throw new NullReferenceException("Post id cannot be null or empity");
            }
            var post = await postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }
            var comments = await commentRepository.GetByPostIdAsync(postId);
            if (comments == null)
            {
                throw new KeyNotFoundException("Comments not found");
            }
            return mapper.Map<IEnumerable<CommentDto>>(comments);
        }
    }
}