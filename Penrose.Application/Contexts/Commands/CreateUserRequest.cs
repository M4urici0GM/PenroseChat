using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Penrose.Application.Contexts.Validators;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Interfaces;
using Penrose.Core.Entities;
using Penrose.Core.Exceptions;
using Penrose.Core.Interfaces.Repositories;

namespace Penrose.Application.Contexts.Commands
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

            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IHashingService _hashingService;
            
            public CreateUserRequestHandler(
                IUserRepository userRepository,
                IMapper mapper,
                IHashingService hashingService)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _hashingService = hashingService;
            }
            
            public async Task<UserDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
            {
                CreateUserDtoValidator dtoValidator = new CreateUserDtoValidator();
                ValidationResult validationResult = await dtoValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                    throw new EntityValidationException(nameof(User), request, validationResult.Errors);

                bool userAlreadyExists = await _userRepository.NicknameExists(request.Nickname);
                if (userAlreadyExists)
                    throw new EntityAlreadyExistsException(nameof(User), request.Nickname);

                User user = _mapper.Map<User>(request);
                user.Hash = await _hashingService.HashStringAsync(request.Password);

                User createdUser = await _userRepository.SaveAsync(user);
                return _mapper.Map<UserDto>(createdUser);
            }
        }
    }
}