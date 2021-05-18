using System;

namespace Penrose.Application.DataTransferObjects
{
    public class JwtTokenDto
    {
        public DateTime CreatedAt { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}