using Microsoft.AspNetCore.Identity;
using Space.Domain.Entities;

namespace Space.Domain.Identity
{
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public string? Bio { get; set; }
        public string? ProfileImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
    }
}