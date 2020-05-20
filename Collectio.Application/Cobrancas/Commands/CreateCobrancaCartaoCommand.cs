using System;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.CartoesCredito.CommandValidators;
using Collectio.Application.Clientes.CommandValidators;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Application.ConfiguracoesEmissao.CommandValidators;
using Collectio.Application.Profiles;
using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.ClienteAggregate;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using FluentValidation;

namespace Collectio.Application.Cobrancas.Commands
{
    public class CreateCobrancaCartaoCommand : ICommand<CobrancaViewModel>, IMapTo<Cobranca>
    {
        public string ConfiguracaoEmissaoId { get; set; }

        public string CartaoCreditoId { get; set; }

        public string ClienteId { get; set; }

        public DateTime Vencimento { get; set; }

        public decimal Valor { get; set; }

        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCobrancaCartaoCommand, Cobranca>()
                .ConstructUsing(c => Cobranca.Cartao(c.Valor, c.Vencimento, c.ClienteId, c.ConfiguracaoEmissaoId, c.CartaoCreditoId));
    }

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