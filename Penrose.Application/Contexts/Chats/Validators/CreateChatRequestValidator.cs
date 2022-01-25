using System.Linq;
using FluentValidation;
using Penrose.Application.Contexts.Chats.Commands;
using Penrose.Core.Structs;

namespace Penrose.Application.Contexts.Chats.Validators
{
  public class CreateChatRequestValidator : AbstractValidator<CreateChatRequest>
  {
    public CreateChatRequestValidator()
    {
      RuleFor(x => x.ChatProperties)
        .NotEmpty()
        .WithMessage(ValidatorDefaultErrorMessages.RequiredField);

      RuleFor(x => x.Participants)
        .NotEmpty()
        .WithMessage(ValidatorDefaultErrorMessages.RequiredField);

      When(x => x.Participants is not null, () =>
      {
        RuleFor(x => x.Participants)
          .NotEmpty()
          .Must(x => x?.ToList().Count > 0)
          .When(x => x.ChatProperties?.Type == Core.Enums.ChatType.Group)
          .WithMessage(ValidatorDefaultErrorMessages.MinListLengthNotSatified)
          .Must(x => x?.ToList().Count == 1)
          .When(x => x.ChatProperties?.Type == Core.Enums.ChatType.Private)
          .WithMessage(ValidatorDefaultErrorMessages.MinListLengthNotSatified);
      });

      When(x => x.ChatProperties is not null, () =>
      {
        RuleFor(x => x.ChatProperties.Type)
          .NotEmpty()
          .WithMessage(ValidatorDefaultErrorMessages.RequiredField);
      });
      
    }
  }
}