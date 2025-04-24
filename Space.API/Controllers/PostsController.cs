using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Space.Application.DTOs.Posts;
using Space.Application.Interfaces.Services;

namespace Space.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly ICurrentUserService currentUserService;

        public PostsController(IPostService postService, ICurrentUserService currentUserService)
        {
            this.postService = postService;
            this.currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var post = await postService.GetPostByIdAsync(id);
            if (post == null)
                throw new KeyNotFoundException("Post not found.");
            return Ok(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePostDto dto)
        {
            await postService.CreatePostAsync(dto);
            return Ok(new { message = "Post created successfully." });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePostDto dto)
        {
            await postService.UpdatePostAsync(dto);
            return Ok(new { message = "Post updated successfully." });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await postService.DeletePostAsync(id);
            return Ok(new { message = "Post deleted successfully." });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyPosts()
        {
            var userId = currentUserService.GetUserId();
            var posts = await postService.GetPostsByUserIdAsync(userId);
            return Ok(posts);
        }
    }
}