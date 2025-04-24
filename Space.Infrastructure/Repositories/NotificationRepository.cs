using Microsoft.EntityFrameworkCore;
using Space.Application.Interfaces.Repositories;
using Space.Domain.Entities;
using Space.Infrastructure.Data;

namespace Space.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext context;

        public NotificationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();
        }

        public async Task<Notification> GetByIdAsync(string notificationId)
        {
            return await context.Notifications
                .Include(n => n.User)
                .Include(n => n.Actor)
                .Where(n => n.Id == notificationId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(string userId)
        {
            return await context.Notifications
             .Include(n => n.User)
             .Include(n => n.Actor)
             .Where(n => n.UserId == userId)
             .OrderByDescending(n => n.CreatedAt)
             .ToListAsync();
        }

        public async Task MarkAsReadAsync(string notificationId)
        {
            var notification = await context.Notifications.FindAsync(notificationId);

            notification.IsRead = true;
            await context.SaveChangesAsync();
        }
    }
}