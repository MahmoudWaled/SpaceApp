using Space.Domain.Entities;

namespace Space.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);

        Task<List<Message>> GetChatAsync(string userId, string otherUserId);

        Task<Message> GetMessageByIdAsync(string messageId);

        Task UpdateMessageAsync(Message message);
    }
}