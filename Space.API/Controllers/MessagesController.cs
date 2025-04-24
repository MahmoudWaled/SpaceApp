using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Space.Application.Interfaces.Services;
using Space.Domain.Entities;

namespace Space.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly ICurrentUserService currentUserService;

        public MessagesController(IMessageService messageService, ICurrentUserService currentUserService)
        {
            this.messageService = messageService;
            this.currentUserService = currentUserService;
        }

        [Authorize]
        [HttpGet("{userId}/{otherUserId}")]
        public async Task<ActionResult<List<Message>>> GetMessages(string userId, string otherUserId)
        {
            var currentUserId = currentUserService.GetUserId();
            var messages = await messageService.GetMessagesAsync(currentUserId, otherUserId);
            return Ok(messages);
        }
    }
}