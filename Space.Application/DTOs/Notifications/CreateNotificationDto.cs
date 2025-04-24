namespace Space.Application.DTOs.Notifications
{
    public class CreateNotificationDto
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string ActorId { get; set; }
        public string EntityId { get; set; }
    }
}