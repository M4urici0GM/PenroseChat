using Microsoft.IdentityModel.Tokens;

namespace Penrose.Application.Interfaces
{
    public interface IJwtSigningKey
    {
        SigningCredentials SigningCredentials { get; set; }
    }
}