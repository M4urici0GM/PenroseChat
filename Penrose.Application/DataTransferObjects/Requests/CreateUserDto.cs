namespace Penrose.Application.DataTransferObjects.Requests
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}