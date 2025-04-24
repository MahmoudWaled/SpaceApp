using AutoMapper;
using Space.Application.DTOs.Notifications;
using Space.Application.Interfaces.Repositories;
using Space.Application.Interfaces.Services;
using Space.Domain.Entities;

namespace Space.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper, IUserRepository userRepository)
        {
            this.notificationRepository = notificationRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task CreateNotificationAsync(string actorId, CreateNotificationDto createNotificationDto)
        {
            if (string.IsNullOrEmpty(actorId))
            {
                throw new ArgumentNullException("User Id cannot be null or empty.");
            }
            if (createNotificationDto == null)
            {
                throw new ArgumentNullException(" create Notification data connot be null.");
            }
            var userWhoIsNotified = await userRepository.FindByIdAsync(createNotificationDto.UserId);
            if (userWhoIsNotified == null)
            {
                throw new KeyNotFoundException("User who is notified not found.");
            }
            if (string.IsNullOrEmpty(createNotificationDto.Type))
            {
                throw new ArgumentNullException("Notification type cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(createNotificationDto.EntityId))
            {
                throw new ArgumentNullException("Entity Id cannot be null or empty.");
            }
            var notification = mapper.Map<Notification>(createNotificationDto);
            notification.ActorId = actorId;
            notification.UserId = createNotificationDto.UserId;
            notification.IsRead = false;
            await notificationRepository.AddAsync(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User Id cannot be null or empty.");
            }
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            var notifications = await notificationRepository.GetByUserIdAsync(userId);
            if (notifications == null || !notifications.Any())
            {
                throw new KeyNotFoundException("No notifications found for this user.");
            }
            var notificationDtos = mapper.Map<IEnumerable<NotificationDto>>(notifications);
            return notificationDtos;
        }

        public async Task MarkNotificationAsReadAsync(string notificationId)
        {
            if (string.IsNullOrEmpty(notificationId))
            {
                throw new ArgumentNullException("Notification Id cannot be null or empty.");
            }
            var notification = await notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new KeyNotFoundException("Notification not found.");
            }
            if (notification.IsRead)
            {
                throw new InvalidOperationException("Notification is already marked as read.");
            }
            await notificationRepository.MarkAsReadAsync(notificationId);
        }
    }
}