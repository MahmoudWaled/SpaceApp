using AutoMapper;
using Space.Application.DTOs.Messages;
using Space.Application.DTOs.Notifications;
using Space.Application.Interfaces.Repositories;
using Space.Application.Interfaces.Services;
using Space.Domain.Entities;

namespace Space.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly INotificationService notificationService;
        private readonly IUserService userService;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper, INotificationService notificationService)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.notificationService = notificationService;
        }

        public async Task SendMessageAsync(CreateMessageDto dto)
        {
            if (string.IsNullOrEmpty(dto.SenderId) || string.IsNullOrEmpty(dto.ReceiverId) || dto.Content == null)
            {
                throw new ArgumentException("SenderId, ReceiverId, and Content cannot be null or empty.");
            }
            var sender = await userRepository.FindByIdAsync(dto.SenderId);
            if (sender == null)
            {
                throw new KeyNotFoundException("Sender not found.");
            }
            var receiver = await userRepository.FindByIdAsync(dto.ReceiverId);
            if (receiver == null)
            {
                throw new KeyNotFoundException("Receiver not found.");
            }

            var message = mapper.Map<Message>(dto);

            await messageRepository.AddMessageAsync(message);
            if (dto.SenderId != dto.ReceiverId)
            {
                var newMessage = await messageRepository.GetChatAsync(dto.SenderId, dto.ReceiverId);
                await notificationService.CreateNotificationAsync(
                    dto.SenderId,
                    new CreateNotificationDto
                    {
                        UserId = dto.ReceiverId,
                        ActorId = dto.SenderId,
                        Type = "Message",
                        EntityId = newMessage.LastOrDefault().Id,
                        Title = $"{sender.UserName} sent you a new message! ({newMessage.LastOrDefault().Content.Substring(0, newMessage.LastOrDefault().Content.Length > 20 ? 20 : newMessage.LastOrDefault().Content.Length) + "..."})"
                    }

                );
            }
        }

        public async Task<List<MessageDto>> GetMessagesAsync(string userId, string otherUserId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NullReferenceException("User Id cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(otherUserId))
            {
                throw new NullReferenceException("Other User Id cannot be null or empty.");
            }
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            var otherUser = await userRepository.FindByIdAsync(otherUserId);
            if (otherUser == null)
            {
                throw new KeyNotFoundException("Other user not found.");
            }
            var messages = await messageRepository.GetChatAsync(userId, otherUserId);
            if (messages == null || messages.Count == 0)
            {
                throw new KeyNotFoundException("No messages found.");
            }
            return mapper.Map<List<MessageDto>>(messages);
        }

        public async Task<Message> MarkMessageAsSeenAsync(string messageId)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                throw new NullReferenceException("Message ID cannot be null or empty.");
            }
            var message = await messageRepository.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                throw new KeyNotFoundException("Message not found.");
            }
            message.IsSeen = true;
            await messageRepository.UpdateMessageAsync(message);
            return message;
        }
    }
}