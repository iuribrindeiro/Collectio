using Collectio.Domain.Base;
using Collectio.Domain.ClienteAggregate.CartaoCreditoModels;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class TransacaoCartaoAprovadaEvent : IDomainEvent
    {
        private Transacao _transacao;

        public Transacao Transacao => _transacao;

        public TransacaoCartaoAprovadaEvent(Transacao transacao) 
            => _transacao = transacao;
    }
}