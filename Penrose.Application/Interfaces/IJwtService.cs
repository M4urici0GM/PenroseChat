using System.Security.Claims;
using Penrose.Application.DataTransferObjects;

namespace Penrose.Application.Interfaces
{
    public interface IJwtService
    {
        JwtTokenDto GenerateToken(ClaimsIdentity userIdentity);
    }
}