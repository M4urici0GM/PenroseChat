using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.DataTransferObjects.Requests;
using Penrose.Application.DataTransferObjects.Validators;
using Penrose.Application.Interfaces;
using Penrose.Core.Common;
using Penrose.Core.Entities;
using Penrose.Core.Exceptions;
using Penrose.Core.Interfaces.Repositories;

namespace Penrose.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHashingService _hashingService;
        
        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IHashingService hashingService)
        {
            _userRepository = userRepository;
            _hashingService = hashingService;
            _mapper = mapper;
        }

        public async Task<UserDto> FindById(Guid id)
        {
            User user = await _userRepository.Find(id);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), id);

            return _mapper.Map<UserDto>(user);
        }
        
        public async Task<UserDto> Create(CreateUserDto userDto)
        {
            CreateUserDtoValidator dtoValidator = new CreateUserDtoValidator();
            ValidationResult validationResult = await dtoValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
                throw new EntityValidationException(nameof(User), userDto, validationResult.Errors);

            bool userAlreadyExists = await _userRepository.NicknameExists(userDto.Nickname);
            if (userAlreadyExists)
                throw new EntityAlreadyExistsException(nameof(User), userDto.Nickname);

            User mappedUser = await MapFromCreateUserDto(userDto);
            User createdUser = await _userRepository.SaveAsync(mappedUser);
            return _mapper.Map<UserDto>(createdUser);
        }

        private async Task<User> MapFromCreateUserDto(CreateUserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            user.Hash = await _hashingService.HashStringAsync(userDto.Password);

            return user;
        }

        public async Task<PagedResult<UserDto>> FindAllAsync(PagedRequest pagedRequest, CancellationToken cancellationToken = new CancellationToken())
        {
            PagedResult<User> users = await _userRepository.FindAllAsync(pagedRequest, CancellationToken.None);
            return new PagedResult<UserDto>()
            {
                Count = users.Count,
                Offset = users.Offset,
                Pagesize = users.Pagesize,
                Records = _mapper.Map<IEnumerable<UserDto>>(users),
            };
        }
    }
}