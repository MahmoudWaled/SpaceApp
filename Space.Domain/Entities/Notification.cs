using Space.Domain.Identity;

namespace Space.Domain.Entities
{
    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string ActorId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? EntityId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; }
        public ApplicationUser Actor { get; set; }
    }
}