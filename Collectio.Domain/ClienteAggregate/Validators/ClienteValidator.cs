using FluentValidation;

namespace Collectio.Domain.ClienteAggregate.Validators
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(e => e.Nome).NotEmpty().NotNull();
        }
    }
}