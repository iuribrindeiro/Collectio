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

                return await cartaoCreditoRepository.ExistsAsync(cartaoCreditoId);
            }).WithMessage("O cartão de crédito informado não existe");
        }

        public static IRuleBuilderOptions<T, string> CartaoCreditoAtivo<T>(this IRuleBuilder<T, string> ruleBuilder, ICartaoCreditoRepository cartaoCreditoRepository)
        {
            return ruleBuilder.Must(id =>
            {
                if (!Guid.TryParse(id, out Guid cartaoCreditoId))
                    return false;

                return cartaoCreditoRepository.Exists(cartaoCreditoId, out CartaoCredito cartaoCredito) && cartaoCredito.ProcessamentoFinalizado;
            }).WithMessage("O cartão de crédito informado não está ativo");
        }
    }
}