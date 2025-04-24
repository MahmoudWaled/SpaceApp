using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Space.Application.DTOs.Follows;
using Space.Application.DTOs.Users;
using Space.Application.Interfaces.Services;

namespace Space.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowsController : ControllerBase
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IFollowService followService;

        public FollowsController(IFollowService followService, ICurrentUserService currentUserService)
        {
            this.followService = followService;
            this.currentUserService = currentUserService;
        }

        [Authorize]
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] FollowRequestDto dto)
        {
            var currentUserId = currentUserService.GetUserId();

            await followService.FollowUserAsync(currentUserId, dto.FolloweeId);

            return Ok("User followed.");
        }

        [HttpGet("followers/{userId}")]
        public async Task<IActionResult> GetFollowers(string userId)
        {
            var followers = await followService.GetFollowersAsync(userId);
            return Ok(followers);
        }

        [HttpGet("following/{userId}")]
        public async Task<IActionResult> GetFollowing(string userId)
        {
            var following = await followService.GetFollowingAsync(userId);
            return Ok(following);
        }

        [Authorize]
        [HttpPost("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] UnfollowRequestDto dto)
        {
            var currentUserId = currentUserService.GetUserId();

            await followService.UnfollowUserAsync(currentUserId, dto.FolloweeId);

            return Ok("User unfollowed.");
        }
    }
}