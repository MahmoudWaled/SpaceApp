namespace Space.Domain.Identity
{
    public interface IApplicationUser
    {
        string Id { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string? ProfileImagePath { get; set; }
    }
}