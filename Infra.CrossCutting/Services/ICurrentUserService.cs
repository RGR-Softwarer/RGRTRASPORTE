namespace Infra.CrossCutting.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        string? UserEmail { get; }
        string? UserRole { get; }
        string? IpAddress { get; }
        bool IsAuthenticated { get; }
    }
} 