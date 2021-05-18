namespace Penrose.Application.DataTransferObjects
{
    public class AuthenticatedUserDto
    {
        public JwtTokenDto JwtToken { get; set; }
        public UserDto User { get; set; }
    }
}