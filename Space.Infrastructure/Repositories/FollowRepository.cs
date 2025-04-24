using Microsoft.EntityFrameworkCore;
using Space.Application.Interfaces.Repositories;
using Space.Domain.Entities;
using Space.Domain.Identity;
using Space.Infrastructure.Data;

namespace Space.Infrastructure.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly AppDbContext context;

        public FollowRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followeeId)
        {
            return await context.Follows
                .AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
        }

        public async Task<bool> AddFollowAsync(Follow follow)
        {
            context.Follows.Add(follow);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveFollowAsync(string followerId, string followeeId)
        {
            var follow = await context.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
            if (follow == null)
                return false;
            context.Follows.Remove(follow);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<IApplicationUser>> GetFollowersAsync(string userId)
        {
            return await context.Users
                .Where(u => u.Following.Any(f => f.FolloweeId == userId))
                .ToListAsync();
        }

        public async Task<IEnumerable<IApplicationUser>> GetFollowingAsync(string userId)
        {
            return await context.Users
                .Where(u => u.Followers.Any(f => f.FollowerId == userId))
                .ToListAsync();
        }
    }
}