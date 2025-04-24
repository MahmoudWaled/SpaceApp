using Space.Domain.Identity;

namespace Space.Domain.Entities
{
    public class Follow
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ApplicationUser Followee { get; set; }
        public string FolloweeId { get; set; }
        public ApplicationUser Follower { get; set; }
        public string FollowerId { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}