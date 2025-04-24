using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Space.Application.Interfaces.Services;

namespace Space.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService notificationService;
        private readonly ICurrentUserService currentUserService;

        public NotificationsController(INotificationService notificationService, ICurrentUserService currentUserService)
        {
            this.notificationService = notificationService;
            this.currentUserService = currentUserService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = currentUserService.GetUserId();
            var notifications = await notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [Authorize]
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(string notificationId)
        {
            await notificationService.MarkNotificationAsReadAsync(notificationId);
            return Ok();
        }
    }
}