using AutoMapper;
using Penrose.Application.Contexts.Users.Commands;
using Penrose.Application.DataTransferObjects;
using Penrose.Core.Entities;

namespace Penrose.Application.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            Users();
        }

        private void Users()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<CreateUserRequest, User>();
            CreateMap<CreateUserRequest, UserDto>();
        }
    }
}