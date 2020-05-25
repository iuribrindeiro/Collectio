using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.Events;

namespace Collectio.Domain.CartaoCreditoAggregate.EventHandlers
{
    public class TransacaoCobrancaReprocessandoEventHandler : IDomainEventHandler<TransacaoCobrancaReprocessandodoEvent>
    {
        private readonly ICobrancasRepository _cobrancasRepository;
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public TransacaoCobrancaReprocessandoEventHandler(ICobrancasRepository cobrancasRepository, ICartaoCreditoRepository cartaoCreditoRepository)
        {
            _cobrancasRepository = cobrancasRepository;
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

        public async Task Handle(TransacaoCobrancaReprocessandodoEvent domainEvent, CancellationToken cancellationToken)
        {
            var cobranca = await _cobrancasRepository.FindAsync(Guid.Parse(domainEvent.CobrancaId));
            if (cobranca.FormaPagamentoCartao && Guid.TryParse(cobranca.ClienteCobranca.CartaoCredito.TenantId, out Guid cartaoCreditoId))
            {
                var cartaoCredito = await _cartaoCreditoRepository.FindAsync(cartaoCreditoId);
                if (cartaoCredito && cartaoCredito.Transacoes.Any(t => t.CobrancaId == domainEvent.CobrancaId))
                {
                    var transacao = cartaoCredito.Transacoes.First(t => t.CobrancaId == domainEvent.CobrancaId);
                    transacao.Reprocessar(cobranca.Valor, cobranca.ConfiguracaoEmissaoId);
                }
            }
        }
    }
}