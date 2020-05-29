using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Cobrancas.CommandValidators;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Collectio.Domain.CartaoCreditoAggregate;

namespace Collectio.Application.Cobrancas.Commands
{
    public class CreateCobrancaCartaoCommand : BaseCreateCobrancaCommand, ICommand<string>, IMapping
    {
        [Required]
        public CartaoCreditoCobrancaViewModel CartaoCredito { get; set; }

        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCobrancaCartaoCommand, Cobranca>()
                .ConstructUsing((c, context) => Cobranca.Cartao(
                    c.Descricao, c.Valor, c.Vencimento, c.ConfiguracaoEmissorId,
                    c.Cliente.Nome, c.Cliente.CpfCnpj, c.Cliente.Email,
                    context.Mapper.Map<Telefone>(c.Cliente.Telefone), context.Mapper.Map<CartaoCreditoCobranca>(c.CartaoCredito), 
                    context.Mapper.Map<Endereco>(c.Cliente.Endereco), c.Cliente.TenantId));
    }

    public class CreateCobrancaCartaoCommandValidator : BaseCreateCobrancaCommandValidator<CreateCobrancaCartaoCommand>
    {
        public CreateCobrancaCartaoCommandValidator(IConfiguracaoEmissaoRepository configuracaoEmissaoRepository, ICartaoCreditoRepository cartaoCreditoRepository)
            : base(configuracaoEmissaoRepository)
        {
            RuleFor(c => c.CartaoCredito).NotNull()
                .WithMessage("Informe o cartão de crédito ao qual deseja efetuar a cobrança")
                .SetValidator(new CartaoCreditoValidator(cartaoCreditoRepository));
        }
    }
}