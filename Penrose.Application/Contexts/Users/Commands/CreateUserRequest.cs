using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Penrose.Application.Contexts.Users.Validators;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Extensions;
using Penrose.Application.Interfaces;
using Penrose.Core.Entities;
using Penrose.Core.Exceptions;
using Penrose.Core.Interfaces.UserStrategies;

namespace Penrose.Application.Contexts.Users.Commands
{
    public class CreateUserRequest : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, UserDto>
        {

            private readonly IUserDataStragegy _userDataStragegy;
            private readonly IMapper _mapper;
            private readonly IHashingService _hashingService;
            
            public CreateUserRequestHandler(
                IUserDataStragegy userDataStragegy,
                IMapper mapper,
                IHashingService hashingService)
            {
                _userDataStragegy = userDataStragegy;
                _mapper = mapper;
                _hashingService = hashingService;
            }
            
            public async Task<UserDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
            {
                await ValidateRequest(request, cancellationToken);
                await CheckIfUserExists(request.Nickname, cancellationToken);
                User user = await MapUser(request);
                
                await _userDataStragegy.SaveAsync(user);
                return _mapper.Map<UserDto>(user);
            }

            private async Task<User> MapUser(CreateUserRequest request)
            {
                User user = _mapper.Map<User>(request);
                user.Hash = await _hashingService.HashStringAsync(request.Password);
                return user;
            }

            private async Task CheckIfUserExists(string nickname, CancellationToken cancellationToken)
            {
                bool userAlreadyExists = await _userDataStragegy.NicknameExistsAsync(nickname, cancellationToken);
                if (userAlreadyExists)
                    throw new EntityAlreadyExistsException(nameof(User), nickname);
            }
            
            private async Task ValidateRequest(CreateUserRequest request, CancellationToken cancellationToken)
            {
                CreateUserDtoValidator dtoValidator = new CreateUserDtoValidator();
                await dtoValidator.ValidateRequest(request, nameof(User), cancellationToken);
            }
        }
    }
}