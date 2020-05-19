using Collectio.Application.CartoesCredito.CommandValidators;
using Collectio.Application.Clientes.CommandValidators;
using Collectio.Application.Cobrancas.Commands;
using Collectio.Application.ConfiguracaoEmissao.CommandValidators;
using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.ClienteAggregate;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using FluentValidation;
using System;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public class CreateCobrancaCartaoCommandValidator : AbstractValidator<CreateCobrancaCartaoCommand>
    {
        public CreateCobrancaCartaoCommandValidator(
            IConfiguracaoEmissaoRepository configuracaoEmissaoRepository, 
            IClientesRepository clientesRepository,
            ICartaoCreditoRepository cartaoCreditoRepository)
        {
            RuleFor(c => c.Valor).GreaterThan(0);
            RuleFor(c => c.ConfiguracaoEmissaoId).ExisteConfiguracaoEmissaoComId(configuracaoEmissaoRepository);
            RuleFor(c => c.ClienteId).ExisteClienteComId(clientesRepository);
            RuleFor(c => c.CartaoCreditoId).ExisteCartaoCreditoComId(cartaoCreditoRepository);
            RuleFor(c => c.Vencimento).GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}