using FluentValidation;

namespace Penrose.Infrastructure.Options.RabbitMq
{
    public class RabbitMqConfigurationOptionsValidator : AbstractValidator<RabbitMqConfigurationOptions>
    {
        public RabbitMqConfigurationOptionsValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.HostName).NotEmpty();
            RuleFor(x => x.VirtualHost).NotEmpty();
        }
    }
}