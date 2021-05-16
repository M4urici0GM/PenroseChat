using System.Security.Authentication;
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
    public class AuthenticateRequest : IRequest<AuthenticatedUserDto>
    {
        public string Nickname { get; set; }
        public string Password { get; set; }
        
        public class AuthenticateRequestHandler : IRequestHandler<AuthenticateRequest, AuthenticatedUserDto>
        {
            private readonly IHashingService _hashingService;
            private readonly IUserDataStragegy _userDataStragegy;
            private readonly IMapper _mapper;

            public AuthenticateRequestHandler(
                IHashingService hashingService,
                IUserDataStragegy userDataStragegy,
                IMapper mapper)
            {
                _hashingService = hashingService;
                _userDataStragegy = userDataStragegy;
                _mapper = mapper;
            }
            
            public async Task<AuthenticatedUserDto> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
            {
                await new AuthenticateRequestValidator().ValidateRequest(request, nameof(User), cancellationToken);
                User user = await FindUser(request.Nickname, cancellationToken);
                await ValidatePassword(user, request.Password);

                return new AuthenticatedUserDto
                {
                    User = _mapper.Map<UserDto>(user),
                };
            }

            private async Task<User> FindUser(string nickname, CancellationToken cancellationToken)
            {
                User user = await _userDataStragegy.FindByNicknameAsync(nickname, cancellationToken);
                if (user == null)
                    throw new EntityNotFoundException(nameof(User), nickname);

                return user;
            }

            private async Task ValidatePassword(User user, string providedPassword)
            {
                bool isPasswordValid = await _hashingService.CompareHashStringAsync(user.Hash, providedPassword);
                if (!isPasswordValid)
                    throw new InvalidCredentialException("Invalid credentials!");
            }
        }
    }
}