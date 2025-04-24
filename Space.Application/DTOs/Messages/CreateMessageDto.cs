namespace Space.Application.DTOs.Messages
{
    public class CreateMessageDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}