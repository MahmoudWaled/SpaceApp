using Microsoft.EntityFrameworkCore;
using Space.Application.Interfaces.Repositories;
using Space.Domain.Entities;
using Space.Infrastructure.Data;

namespace Space.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext context;

        public PostRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Post post)
        {
            await context.Posts.AddAsync(post);
        }

        public async Task UpdateAsync(Post post)
        {
            context.Posts.Update(post);
        }

        public async Task DeleteAsync(Post post)
        {
            post.IsDeleted = true;
            context.Posts.Update(post);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await context.Posts

            .Include(p => p.Comments).ThenInclude(c => c.User)
            .Include(p => p.Likes).ThenInclude(l => l.User)
            .Include(p => p.User)
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(string id)
        {
            return await context.Posts
            .Include(p => p.Comments).ThenInclude(c => c.User)
        .Include(p => p.Likes).ThenInclude(l => l.User)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<IEnumerable<Post>> GetByUserIdAsync(string userId)
        {
            return await context.Posts
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Likes).ThenInclude(l => l.User)
                .Include(p => p.User)
                .Where(p => p.User.Id == userId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}