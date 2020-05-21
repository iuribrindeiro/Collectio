using System;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Application.Profiles;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.ClienteAggregate;
using FluentValidation;

namespace Collectio.Application.Cobrancas.Commands
{
    public class CreateCobrancaBoletoCommand : BaseCreateCobrancaCommand, ICommand<CobrancaViewModel>, IMapTo<Cobranca>
    {
        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCobrancaBoletoCommand, Cobranca>()
                .ConstructUsing((c, context) => Cobranca.Boleto(
                    c.Valor, c.Vencimento, c.ConfiguracaoEmissorId, 
                    c.NomeCliente, c.CpfCnpjCliente, c.EmailCliente,
                    context.Mapper.Map<Telefone>(c.TelefoneCliente), 
                    context.Mapper.Map<Endereco>(c.EnderecoCliente), 
                    c.TenantIdCliente));
    }

    public class CreateCobrancaBoletoValidator : AbstractValidator<CreateCobrancaBoletoCommand>
    {
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly IClientesRepository _clientesRepository;

        public CreateCobrancaBoletoValidator(ICartaoCreditoRepository cartaoCreditoRepository, IClientesRepository clientesRepository)
        {
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _clientesRepository = clientesRepository;

            RuleFor(c => c.NomeCliente)
                .MustAsync(async (c, a) => await _clientesRepository.FindAsync(Guid.Parse(c))).WithMessage("Cliente selecionado não existe");
        }
    }
}
