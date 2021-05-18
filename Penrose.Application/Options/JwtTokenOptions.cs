namespace Penrose.Application.Options
{
    public class JwtTokenOptions
    {
        public string SecurityKey { get; set; }
        public int ExpirationTime { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}