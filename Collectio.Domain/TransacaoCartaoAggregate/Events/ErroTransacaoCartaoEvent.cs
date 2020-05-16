using Collectio.Domain.Base;

namespace Collectio.Domain.TransacaoCartaoAggregate.Events
{
    public class ErroTransacaoCartaoEvent : IDomainEvent
    {
        private TransacaoCartao _transacaoCartao;

        public TransacaoCartao TransacaoCartao => _transacaoCartao;

        public ErroTransacaoCartaoEvent(TransacaoCartao transacaoCartao)
            => _transacaoCartao = transacaoCartao;
    }
}