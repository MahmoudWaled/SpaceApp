namespace Space.Application.DTOs.Comments
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string ProfileImgPath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}