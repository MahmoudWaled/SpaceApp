using Space.Application.DTOs.Messages;
using Space.Domain.Entities;

namespace Space.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(CreateMessageDto dto);

        Task<List<MessageDto>> GetMessagesAsync(string userId, string otherUserId);

        Task<Message> MarkMessageAsSeenAsync(string messageId);
    }
}