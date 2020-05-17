using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class ReprocessandoTransacaoCartaoEvent : IDomainEvent
    {
        private Transacao _transacao;
        private Transacao _transacaoAnterior;

        public Transacao Transacao => _transacao;
        public Transacao TransacaoAnterior => _transacaoAnterior;

        public ReprocessandoTransacaoCartaoEvent(Transacao transacao, Transacao transacaoAnterior)
        {
            _transacao = transacao;
            _transacaoAnterior = transacaoAnterior;
        }
    }
}