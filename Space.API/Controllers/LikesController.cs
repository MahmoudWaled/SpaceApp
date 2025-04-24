using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Space.Application.DTOs.Likes;
using Space.Application.Interfaces.Services;

namespace Space.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService likeService;
        private readonly ICurrentUserService currentUserService;

        public LikesController(ILikeService likeService, ICurrentUserService currentUserService)
        {
            this.likeService = likeService;
            this.currentUserService = currentUserService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LikePost([FromBody] LikeDto dto)
        {
            dto.UserId = currentUserService.GetUserId();
            await likeService.LikePost(dto);
            return Ok("Post liked.");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> UnlikePost([FromBody] LikeDto dto)
        {
            dto.UserId = currentUserService.GetUserId();
            await likeService.UnlikePost(dto);
            return Ok("Post unliked.");
        }

        [HttpGet("count/{postId}")]
        public async Task<IActionResult> GetLikesCount(string postId)
        {
            var count = await likeService.CountLikesOnPostAsync(postId);
            return Ok(count);
        }
    }
}