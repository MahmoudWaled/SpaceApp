namespace Space.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string? GetUserId();

        string? GetUserName();

        string? GetUserEmail();
    }
}