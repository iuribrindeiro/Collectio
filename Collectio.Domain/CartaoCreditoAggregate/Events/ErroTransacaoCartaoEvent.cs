using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class ErroTransacaoCartaoEvent : IDomainEvent
    {
        private string _transacaoId;

        public string TransacaoId => _transacaoId;

        public ErroTransacaoCartaoEvent(string transacaoId)
            => _transacaoId = transacaoId;
    }
}