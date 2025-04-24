using Space.Application.DTOs.Notifications;

namespace Space.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string ActorId, CreateNotificationDto createNotificationDto);

        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId);

        Task MarkNotificationAsReadAsync(string notificationId);
    }
}