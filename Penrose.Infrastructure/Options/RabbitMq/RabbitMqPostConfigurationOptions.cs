using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Penrose.Core.Exceptions;

namespace Penrose.Infrastructure.Options.RabbitMq
{
    public class RabbitMqPostConfigurationOptions : IPostConfigureOptions<RabbitMqConfigurationOptions>
    {
        public void PostConfigure(string name, RabbitMqConfigurationOptions options)
        {
            RabbitMqConfigurationOptionsValidator validator = new RabbitMqConfigurationOptionsValidator();
            ValidationResult validationResult = validator.Validate(options);
            if (validationResult.IsValid)
                return;
            
            throw new EntityValidationException(
                nameof(RabbitMqConfigurationOptions),
                nameof(options),
                validationResult.Errors);
        }
    }
}