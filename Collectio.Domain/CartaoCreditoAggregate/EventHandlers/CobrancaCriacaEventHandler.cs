using System;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.Events;

namespace Collectio.Domain.CartaoCreditoAggregate.EventHandlers
{
    public class CobrancaCriacaEventHandler : IDomainEventHandler<CobrancaCriadaEvent>
    {
        private readonly ICobrancasRepository _cobrancasRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public CobrancaCriacaEventHandler(ICobrancasRepository cobrancasRepository, ICartaoCreditoRepository cartaoCreditoRepository)
        {
            _cobrancasRepository = cobrancasRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

        public async Task Handle(CobrancaCriadaEvent domainEvent, CancellationToken cancellationToken)
        {
            var cobranca = await _cobrancasRepository.FindAsync(Guid.Parse(domainEvent.CobrancaId));
            if (cobranca.FormaPagamentoCartao)
            {
                var cartaoCredito = await _cartaoCreditoRepository.FindAsync(Guid.Parse(cobranca.ClienteCobranca.CartaoCredito.TenantId));
                cartaoCredito.AddTransacao(cobranca.Id.ToString(), cobranca.ConfiguracaoEmissaoId, cobranca.Valor);
            }
        }
    }
}
