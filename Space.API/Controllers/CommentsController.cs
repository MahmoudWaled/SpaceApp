using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Space.Application.DTOs.Comments;
using Space.Application.Interfaces.Services;

namespace Space.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly ICurrentUserService currentUserService;

        public CommentsController(ICommentService commentService, ICurrentUserService currentUserService)
        {
            this.commentService = commentService;
            this.currentUserService = currentUserService;
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(string postId)
        {
            var comments = await commentService.GetCommentsByPostIdAsync(postId);
            return Ok(comments);
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentById(string commentId)
        {
            var comment = await commentService.GetCommentByIdAsync(commentId);
            return Ok(comment);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto dto)
        {
            var userId = currentUserService.GetUserId();

            await commentService.AddCommentAsync(userId, dto);
            return Ok("Comment added.");
        }

        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            var userId = currentUserService.GetUserId();
            await commentService.DeleteCommentAsync(userId, commentId);
            return Ok("Comment deleted.");
        }
    }
}