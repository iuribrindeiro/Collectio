using Collectio.Application.Cobrancas.ViewModels;
using FluentValidation;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public class CartaoCreditoValidator : AbstractValidator<CartaoCreditoCobrancaViewModel>
    {
        public CartaoCreditoValidator()
        {
            RuleFor(c => c.Nome).NotEmpty();
            RuleFor(c => c.Numero).NotEmpty().WithName("Número");
        }
    }
}