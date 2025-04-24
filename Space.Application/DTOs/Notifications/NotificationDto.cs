namespace Space.Application.DTOs.Notifications
{
    public class NotificationDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string ActorId { get; set; }
        public string ActorUserName { get; set; }
        public string ActorProfileImage { get; set; }

        public string EntityId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}