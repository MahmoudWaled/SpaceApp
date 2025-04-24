using Microsoft.EntityFrameworkCore;
using Space.Application.Interfaces.Repositories;
using Space.Domain.Entities;
using Space.Infrastructure.Data;

namespace Space.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext context;

        public MessageRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddMessageAsync(Message message)
        {
            context.Messages.Add(message);
            await context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetChatAsync(string userId, string otherUserId)
        {
            return await context.Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                            (m.SenderId == otherUserId && m.ReceiverId == userId))
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(string messageId)
        {
            return await context.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task UpdateMessageAsync(Message message)
        {
            context.Messages.Update(message);
            await context.SaveChangesAsync();
        }
    }
}