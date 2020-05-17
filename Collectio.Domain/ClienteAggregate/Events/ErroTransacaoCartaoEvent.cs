using Collectio.Domain.Base;
using Collectio.Domain.ClienteAggregate.CartaoCreditoModels;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class ErroTransacaoCartaoEvent : IDomainEvent
    {
        private Transacao _transacao;

        public Transacao Transacao => _transacao;

        public ErroTransacaoCartaoEvent(Transacao transacao)
            => _transacao = transacao;
    }
}