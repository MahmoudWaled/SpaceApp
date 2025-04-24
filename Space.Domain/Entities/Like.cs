using Space.Domain.Identity;

namespace Space.Domain.Entities
{
    public class Like
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Post Post { get; set; } = null!;
        public string PostId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}