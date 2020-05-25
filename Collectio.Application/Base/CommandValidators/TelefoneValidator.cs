using Collectio.Application.Base.ViewModels;
using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public class TelefoneValidator : AbstractValidator<TelefoneViewModel>
    {
        public TelefoneValidator()
        {
            RuleFor(t => t.Numero).NotEmpty().IsValidNumeroTelefone();
            RuleFor(t => t.Ddd).NotEmpty().IsValidDddTelefone();
        }
    }
}