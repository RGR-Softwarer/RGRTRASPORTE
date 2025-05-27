#nullable enable

using System.Security.Claims;

namespace Infra.CrossCutting.Auth
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string userName, IEnumerable<string> roles);
        ClaimsPrincipal? ValidateToken(string token);
    }
} 