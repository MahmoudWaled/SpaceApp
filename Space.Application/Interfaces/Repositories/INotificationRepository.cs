using Space.Domain.Entities;

namespace Space.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(string userId);

        Task<Notification> GetByIdAsync(string notificationId);

        Task AddAsync(Notification notification);

        Task MarkAsReadAsync(string notificationId);
    }
}