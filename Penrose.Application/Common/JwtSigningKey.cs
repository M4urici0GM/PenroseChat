using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Penrose.Application.Interfaces;
using Penrose.Application.Options;

namespace Penrose.Application.Common
{
    public class JwtSigningKey : IJwtSigningKey
    {
        public SigningCredentials SigningCredentials { get; set; }

        public JwtSigningKey(JwtTokenOptions jwtTokenOptions)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(jwtTokenOptions.SecurityKey);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);

            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}