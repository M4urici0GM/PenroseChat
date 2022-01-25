using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Penrose.Core.Exceptions;

namespace Penrose.Application.Extensions
{
  public static class AbstractValidatorExtensions
  {
    public static async Task ValidateRequest<T>(
        this AbstractValidator<T> validator,
        T request,
        string entityName,
        CancellationToken cancellationToken)
    {
      ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
      if (!validationResult.IsValid)
        throw new EntityValidationException(entityName, request, validationResult.Errors);
    }
  }
}