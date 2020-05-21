using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Clientes.CommandValidators;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.ClienteAggregate;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using FluentValidation;
using System;
using CartaoCredito = Collectio.Domain.CobrancaAggregate.CartaoCredito;

namespace Collectio.Application.Cobrancas.Commands
{
    public class CreateCobrancaCartaoCommand : BaseCreateCobrancaCommand, ICommand<CobrancaViewModel>, IMapTo<Cobranca>
    {
        public CartaoCreditoViewModel CartaoCredito { get; set; }

        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCobrancaCartaoCommand, Cobranca>()
                .ConstructUsing((c, context) => Cobranca.Cartao(
                    c.Valor, c.Vencimento, c.ConfiguracaoEmissorId, 
                    c.NomeCliente, c.CpfCnpjCliente, c.EmailCliente,
                    context.Mapper.Map<Telefone>(c.TelefoneCliente), context.Mapper.Map<CartaoCredito>(c.CartaoCredito), 
                    context.Mapper.Map<Endereco>(c.EnderecoCliente), c.TenantIdCliente));
    }

    public class CreateCobrancaCartaoCommandValidator : AbstractValidator<CreateCobrancaCartaoCommand>
    {
        public CreateCobrancaCartaoCommandValidator(
            IConfiguracaoEmissaoRepository configuracaoEmissaoRepository,
            IClientesRepository clientesRepository,
            ICartaoCreditoRepository cartaoCreditoRepository)
        {
            RuleFor(c => c.Valor).GreaterThan(0);
            RuleFor(c => c.TenantIdCliente).ExisteClienteComId(clientesRepository);
            RuleFor(c => c.Vencimento).GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}