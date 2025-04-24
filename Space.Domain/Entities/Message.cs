using Space.Domain.Identity;

namespace Space.Domain.Entities
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ApplicationUser Sender { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSeen { get; set; } = false;
    }
}