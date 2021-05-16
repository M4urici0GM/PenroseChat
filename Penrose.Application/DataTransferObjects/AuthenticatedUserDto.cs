namespace Penrose.Application.DataTransferObjects
{
    public class AuthenticatedUserDto
    {
        public WebTokenDto WebToken { get; set; }
        public UserDto User { get; set; }
    }
}