using Space.Domain.Identity;

namespace Space.Domain.Entities
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? TextContent { get; set; }
        public string? ImagePath { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}