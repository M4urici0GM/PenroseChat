using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Interfaces;
using Penrose.Application.Options;

namespace Penrose.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly JwtTokenOptions _jwtTokenOptions;
        private readonly IJwtSigningKey _jwtSigningKey;
        
        public JwtService(
            IOptions<JwtTokenOptions> jwtTokenOptions,
            IJwtSigningKey jwtSigningKey)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _jwtTokenOptions = jwtTokenOptions.Value;
            _jwtSigningKey = jwtSigningKey;
        }

        public JwtTokenDto GenerateToken(ClaimsIdentity userIdentity)
        {
            DateTime authTime = DateTime.UtcNow;
            DateTime expirationTime = authTime.AddSeconds(_jwtTokenOptions.ExpirationTime);
            
            userIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")));
            userIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.AuthTime, authTime.ToString("u")));

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = userIdentity,
                NotBefore = authTime,
                Expires = expirationTime,
                IssuedAt = authTime,
                Audience = _jwtTokenOptions.Audience,
                Issuer = _jwtTokenOptions.Issuer,
                SigningCredentials = _jwtSigningKey.SigningCredentials,
            };
            
            // TODO: Implement Refresh Tokens

            SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
            return new JwtTokenDto()
            {
                Token = _tokenHandler.WriteToken(securityToken),
                CreatedAt = authTime,
                ExpiresIn = _jwtTokenOptions.ExpirationTime,
            };
        }
    }
}