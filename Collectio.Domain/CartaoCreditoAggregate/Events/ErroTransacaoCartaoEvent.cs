using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class ErroTransacaoCartaoEvent : IDomainEvent
    {
        private Transacao _transacao;

        public Transacao Transacao => _transacao;

        public ErroTransacaoCartaoEvent(Transacao transacao)
            => _transacao = transacao;
    }
}