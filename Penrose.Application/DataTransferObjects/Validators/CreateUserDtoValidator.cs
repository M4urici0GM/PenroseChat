using FluentValidation;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Penrose.Application.DataTransferObjects.Requests;
using Penrose.Core.Structs;

namespace Penrose.Application.DataTransferObjects.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ValidationErrorMessages.RequiredField)
                .EmailAddress();

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationErrorMessages.RequiredField);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(ValidationErrorMessages.RequiredField)
                .Matches("^(?=.{6,15}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$")
                .WithMessage("The username must have between 6 and 15 chars, having only letters and numbers.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidationErrorMessages.RequiredField)
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,30}$")
                .WithMessage(
                    @"Password must have at least 6 and maximum of 30 characters, at least one number and one special character");
        }
    }
}   