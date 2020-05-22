using Collectio.Application.Base.CommandValidators;
using Collectio.Application.Cobrancas.ViewModels;
using FluentValidation;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public class TelefoneValidator : AbstractValidator<TelefoneViewModel>
    {
        public TelefoneValidator()
        {
            RuleFor(t => t.Numero).IsValidTelefone();
            RuleFor(t => t.Ddd).IsValidDdd();
        }
    }
}