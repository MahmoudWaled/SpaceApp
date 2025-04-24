using AutoMapper;
using Space.Application.DTOs.Posts;
using Space.Application.Interfaces.Repositories;
using Space.Application.Interfaces.Services;
using Space.Domain.Entities;

namespace Space.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public PostService(IPostRepository postRepository, IMapper mapper, ICurrentUserService currentUserService, IFileService fileService)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task CreatePostAsync(CreatePostDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var post = mapper.Map<Post>(dto);
            var userId = currentUserService.GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new KeyNotFoundException("User not authenticated.");

            post.User.Id = userId;
            if (dto.ImagePath != null)
            {
                post.ImagePath = await fileService.SaveFileAsync(dto.ImagePath, "images");
            }

            await postRepository.AddAsync(post);
            await postRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await postRepository.GetAllAsync();
            if (posts == null)
                throw new NullReferenceException("No posts found");
            return mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<PostDto?> GetPostByIdAsync(string id)
        {
            var post = await postRepository.GetByIdAsync(id);
            if (post == null)
                throw new NullReferenceException("No post found with this post id");
            return mapper.Map<PostDto>(post);
        }

        public async Task UpdatePostAsync(UpdatePostDto dto)
        {
            var post = await postRepository.GetByIdAsync(dto.PostId);
            var currentUserId = currentUserService.GetUserId();

            if (post == null)
                throw new KeyNotFoundException("Post not found.");
            if (post.User.Id != currentUserId)
                throw new UnauthorizedAccessException("You are not authorized to update this post.");
            post.TextContent = dto.TextContent;

            if (dto.ImagePath != null)
            {
                post.ImagePath = await fileService.SaveFileAsync(dto.ImagePath, "images");
            }

            await postRepository.UpdateAsync(post);
            await postRepository.SaveChangesAsync();
        }

        public async Task DeletePostAsync(string id)
        {
            var post = await postRepository.GetByIdAsync(id);
            var currentUserId = currentUserService.GetUserId();

            if (post == null)
                throw new KeyNotFoundException("Post not found.");

            if (post.User.Id != currentUserId)
                throw new UnauthorizedAccessException("You are not authorized to delete this post.");

            post.IsDeleted = true;

            await postRepository.UpdateAsync(post);
            await postRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostDto>> GetPostsByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new KeyNotFoundException("no user found with this id");
            var posts = await postRepository.GetByUserIdAsync(userId);
            if (posts == null)
                throw new NullReferenceException("No posts found with this user id");
            return mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}