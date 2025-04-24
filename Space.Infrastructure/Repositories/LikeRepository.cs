using Microsoft.EntityFrameworkCore;
using Space.Application.Interfaces.Repositories;
using Space.Domain.Entities;
using Space.Infrastructure.Data;

namespace Space.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext context;

        public LikeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Like like)
        {
            await context.Likes.AddAsync(like);
            await context.SaveChangesAsync();
        }

        public async Task<int> CountLikesOnPostAsync(string postId)
        {
            return await context.Likes
                  .Where(l => l.PostId == postId)
                  .CountAsync();
        }

        public async Task<bool> HasUserLikedPostAsync(string userId, string postId)
        {
            return await context.Likes
                .AnyAsync(l => l.UserId == userId && l.PostId == postId);
        }

        public async Task RemoveAsync(Like like)
        {
            context.Likes.Remove(like);
            await context.SaveChangesAsync();
        }

        public async Task<Like?> GetLikeAsync(string userId, string postId)
        {
            return await context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        }
    }
}