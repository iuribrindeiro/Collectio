using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class TransacaoCartaoCriadaEvent : IDomainEvent
    {
        private string _transacaoId;

        public string TransacaoId => _transacaoId;

        public TransacaoCartaoCriadaEvent(string transacaoId) 
            => _transacaoId = transacaoId;
    }
}