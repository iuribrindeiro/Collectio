using System;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using FluentValidation;

namespace Collectio.Application.ConfiguracoesEmissao.CommandValidators
{
    public static class ConfiguracaoEmissaoExisteValidator
    {
        public static IRuleBuilderOptions<T, string> ExisteConfiguracaoEmissaoComId<T>(this IRuleBuilder<T, string> ruleBuilder, IConfiguracaoEmissaoRepository configuracaoEmissaoRepository)
        {
            return ruleBuilder.MustAsync(async (id, b) =>
            {
                if (!Guid.TryParse(id, out Guid configuracaoEmissaoId))
                    return false;

                return await configuracaoEmissaoRepository.Exists(configuracaoEmissaoId);
            }).WithMessage("A configuração de emissão informada não existe");
        }
    }
}