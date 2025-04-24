namespace Space.Application.DTOs.Messages
{
    public class MessageDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string SenderImagePath { get; set; }
        public string ReceiverImagePath { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsSeen { get; set; } = false;
    }
}