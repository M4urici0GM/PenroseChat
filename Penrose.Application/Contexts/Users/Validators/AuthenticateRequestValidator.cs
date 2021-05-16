using FluentValidation;
using Penrose.Application.Contexts.Users.Commands;
using Penrose.Core.Structs;

namespace Penrose.Application.Contexts.Users.Validators
{
    public class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequest>
    {
        public AuthenticateRequestValidator()
        {
            RuleFor(x => x.Nickname)
                .NotEmpty()
                .WithMessage(ValidatorDefaultErrorMessages.RequiredField);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidatorDefaultErrorMessages.RequiredField);
        }
    }
}