using System;
using Collectio.Domain.CartaoCreditoAggregate;
using FluentValidation;

namespace Collectio.Application.CartoesCredito.CommandValidators
{
    public static class CartaoCreditoExisteValidator
    {
        public static IRuleBuilderOptions<T, string> ExisteCartaoCreditoComId<T>(this IRuleBuilder<T, string> ruleBuilder, ICartaoCreditoRepository cartaoCreditoRepository)
        {
            return ruleBuilder.MustAsync(async (id, b) =>
            {
                if (!Guid.TryParse(id, out Guid cartaoCreditoId))
                    return false;

                return await cartaoCreditoRepository.Exists(cartaoCreditoId);
            }).WithMessage("O cartão de crédito informado não existe");
        }
    }
}