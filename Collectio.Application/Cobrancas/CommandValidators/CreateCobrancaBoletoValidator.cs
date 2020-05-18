using System;
using Collectio.Application.Cobrancas.Commands;
using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.ClienteAggregate;
using FluentValidation;

namespace Collectio.Application.Cobrancas.CommandValidators
{
    public class CreateCobrancaBoletoValidator : AbstractValidator<CreateCobrancaBoletoCommand>
    {
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;
        private readonly IClientesRepository _clientesRepository;

        public CreateCobrancaBoletoValidator(ICartaoCreditoRepository cartaoCreditoRepository, IClientesRepository clientesRepository)
        {
            _cartaoCreditoRepository = cartaoCreditoRepository;
            _clientesRepository = clientesRepository;

            RuleFor(c => c.ClienteId)
                .MustAsync(async (c, a) => await _clientesRepository.FindAsync(Guid.Parse(c))).WithMessage("Cliente selecionado não existe");
        }
    }
}
