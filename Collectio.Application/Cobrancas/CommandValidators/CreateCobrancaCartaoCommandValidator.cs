using Collectio.Application.Cobrancas.Commands;
using FluentValidation;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public class CreateCobrancaCartaoCommandValidator : AbstractValidator<CreateCobrancaCartaoCommand>
    {
        public CreateCobrancaCartaoCommandValidator()
        {
            RuleFor(c => c.Valor).GreaterThan(0);
        }
    }
}