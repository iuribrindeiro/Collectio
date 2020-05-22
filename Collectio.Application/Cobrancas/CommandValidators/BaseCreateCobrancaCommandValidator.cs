using System;
using Collectio.Application.Cobrancas.Commands;
using Collectio.Application.ConfiguracoesEmissao.CommandValidators;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using FluentValidation;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public abstract class BaseCreateCobrancaCommandValidator<T> : AbstractValidator<T>
        where T : BaseCreateCobrancaCommand
    {
        public BaseCreateCobrancaCommandValidator(IConfiguracaoEmissaoRepository configuracaoEmissaoRepository)
        {
            RuleFor(c => c.Valor).GreaterThan(0);
            RuleFor(c => c.Vencimento).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(c => c.ConfiguracaoEmissorId).ExisteConfiguracaoEmissaoComId(configuracaoEmissaoRepository);
            RuleFor(c => c.Cliente).SetValidator(new ClienteCobrancaValidator());
        }
    }
}