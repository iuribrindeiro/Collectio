using Collectio.Domain.Base;

namespace Collectio.Domain.TransacaoCartaoAggregate
{
    public class ReprocessandoTransacaoCartaoEvent : IDomainEvent
    {
        private TransacaoCartao _transacaoCartao;
        private TransacaoCartao _transacaoCartaoAnterior;

        public TransacaoCartao TransacaoCartao => _transacaoCartao;
        public TransacaoCartao TransacaoCartaoAnterior => _transacaoCartaoAnterior;

        public ReprocessandoTransacaoCartaoEvent(TransacaoCartao transacaoCartao, TransacaoCartao transacaoCartaoAnterior)
        {
            _transacaoCartao = transacaoCartao;
            _transacaoCartaoAnterior = transacaoCartaoAnterior;
        }
    }
}