using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Penrose.Application.Contexts.Users.Validators;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Extensions;
using Penrose.Application.Interfaces;
using Penrose.Application.Interfaces.UserStrategies;
using Penrose.Core.Entities;
using Penrose.Core.Exceptions;
using Penrose.Core.Structs;

namespace Penrose.Application.Contexts.Users.Commands
{
    public class AuthenticateRequest : IRequest<AuthenticatedUserDto>
    {
        public string Nickname { get; set; }
        public string Password { get; set; }
        
        public class AuthenticateRequestHandler : IRequestHandler<AuthenticateRequest, AuthenticatedUserDto>
        {
            private readonly IHashingService _hashingService;
            private readonly IUserDataStrategy _userDataStrategy;
            private readonly IJwtService _jwtService;
            private readonly IMapper _mapper;

            public AuthenticateRequestHandler(
                IMapper mapper,
                IJwtService jwtService,
                IHashingService hashingService,
                IUserDataStrategy userDataStrategy)
            {
                _hashingService = hashingService;
                _userDataStrategy = userDataStrategy;
                _mapper = mapper;
                _jwtService = jwtService;
            }
            
            public async Task<AuthenticatedUserDto> Handle(
                AuthenticateRequest request,
                CancellationToken cancellationToken)
            {
                await new AuthenticateRequestValidator().ValidateRequest(request, nameof(User), cancellationToken);
                User user = await ValidatePassword(request.Nickname, request.Password, cancellationToken);
                JwtTokenDto userToken = GenerateUserToken(user);

                return new AuthenticatedUserDto
                {
                    User = _mapper.Map<UserDto>(user),
                    JwtToken = userToken,
                };
            }

            private JwtTokenDto GenerateUserToken(User user)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(user.Nickname);
                claimsIdentity.AddClaim(new Claim(PenroseJwtTokenClaimNames.UserId, user.Id.ToString("D")));
                claimsIdentity.AddClaim(new Claim(PenroseJwtTokenClaimNames.DisplayName, $"{user.Name} {user.LastName}"));
                
                return _jwtService.GenerateToken(claimsIdentity);
            }

            private async Task<User> FindUser(string nickname, CancellationToken cancellationToken)
            {
                User user = await _userDataStrategy.FindByNicknameAsync(nickname, cancellationToken);
                if (user == null || !user.IsActive)
                    throw new EntityNotFoundException(nameof(User), nickname);

                return user;
            }

            private async Task<User> ValidatePassword(
                string nickname,
                string providedPassword,
                CancellationToken cancellationToken)
            {
                User user = await FindUser(nickname, cancellationToken);
                bool isPasswordValid = await _hashingService.CompareHashStringAsync(user.Hash, providedPassword);
                if (!isPasswordValid)
                    throw new InvalidCredentialException("Invalid credentials!");

                return user;
            }
        }
    }
}