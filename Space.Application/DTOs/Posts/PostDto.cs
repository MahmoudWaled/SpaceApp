using Space.Application.DTOs.Comments;
using Space.Application.DTOs.Users;

namespace Space.Application.DTOs.Posts
{
    public class PostDto
    {
        public string Id { get; set; }

        public string? TextContent { get; set; }

        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; } = string.Empty;
        public int LikesCount { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public ICollection<string> LikesUserNames { get; set; }
        public ICollection<string> LikesIds { get; set; }

        public UserDto User { get; set; }
    }
}