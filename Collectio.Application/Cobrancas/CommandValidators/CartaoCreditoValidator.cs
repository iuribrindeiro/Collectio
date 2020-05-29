using Collectio.Application.CartoesCredito.CommandValidators;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Domain.CartaoCreditoAggregate;
using FluentValidation;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public class CartaoCreditoValidator : AbstractValidator<CartaoCreditoCobrancaViewModel>
    {
        public CartaoCreditoValidator(ICartaoCreditoRepository cartaoCreditoRepository)
        {
            RuleFor(c => c.TenantId)
                .NotEmpty()
                .ExisteCartaoCreditoComId(cartaoCreditoRepository)
                .CartaoCreditoAtivo(cartaoCreditoRepository);
            RuleFor(c => c.Nome).NotEmpty();
            RuleFor(c => c.Numero).NotEmpty().WithName("Número");
        }
    }
}