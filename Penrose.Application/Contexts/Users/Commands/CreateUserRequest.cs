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

            private readonly IUserDataStrategy _userDataStrategy;
            private readonly IMapper _mapper;
            private readonly IHashingService _hashingService;
            
            public CreateUserRequestHandler(
                IUserDataStrategy userDataStrategy,
                IMapper mapper,
                IHashingService hashingService)
            {
                _userDataStrategy = userDataStrategy;
                _mapper = mapper;
                _hashingService = hashingService;
            }
            
            public async Task<UserDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
            {
                await ValidateRequest(request, cancellationToken);
                await CheckIfUserExists(request.Nickname, request.Email, cancellationToken);
                User user = await MapUser(request);
                
                await _userDataStrategy.SaveAsync(user);
                return _mapper.Map<UserDto>(user);
            }

            private async Task<User> MapUser(CreateUserRequest request)
            {
                User user = _mapper.Map<User>(request);
                user.Hash = await _hashingService.HashStringAsync(request.Password);
                return user;
            }

            private async Task CheckIfUserExists(string nickname, string email, CancellationToken cancellationToken)
            {
                bool userAlreadyExists = await _userDataStrategy.NicknameOrEmailExists(nickname, email, cancellationToken);
                if (userAlreadyExists)
                    throw new EntityAlreadyExistsException(nameof(User), $"{nickname} or {email}");
            }
            
            private async Task ValidateRequest(CreateUserRequest request, CancellationToken cancellationToken)
            {
                CreateUserDtoValidator dtoValidator = new CreateUserDtoValidator();
                await dtoValidator.ValidateRequest(request, nameof(User), cancellationToken);
            }
        }
    }
}