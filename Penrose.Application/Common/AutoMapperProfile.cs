using AutoMapper;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.DataTransferObjects.Requests;
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
            CreateMap<CreateUserDto, User>();
            CreateMap<CreateUserDto, UserDto>();
        }
    }
}