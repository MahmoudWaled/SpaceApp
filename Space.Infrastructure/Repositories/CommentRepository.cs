using Microsoft.EntityFrameworkCore;
using Space.Application.Interfaces.Repositories;
using Space.Domain.Entities;
using Space.Infrastructure.Data;

namespace Space.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext context;

        public CommentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Comment comment)
        {
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }

        public async Task<Comment?> GetByIdAsync(string id)
        {
            return await context.Comments

                .Include(c => c.User)
                .Include(c => c.Post)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(string postId)
        {
            return await context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}