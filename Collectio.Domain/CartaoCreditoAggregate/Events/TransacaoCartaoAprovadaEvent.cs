using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class TransacaoCartaoAprovadaEvent : IDomainEvent
    {
        private Transacao _transacao;

        public Transacao Transacao => _transacao;

        public TransacaoCartaoAprovadaEvent(Transacao transacao) 
            => _transacao = transacao;
    }
}