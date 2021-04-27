using FluentValidation;
using Penrose.Application.Contexts.Commands;
using Penrose.Core.Structs;

namespace Penrose.Application.Contexts.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserRequest>
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