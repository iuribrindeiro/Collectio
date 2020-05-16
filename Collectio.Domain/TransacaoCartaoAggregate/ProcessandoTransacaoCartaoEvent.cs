using Collectio.Domain.Base;

namespace Collectio.Domain.TransacaoCartaoAggregate
{
    public class ProcessandoTransacaoCartaoEvent : IDomainEvent
    {
        private TransacaoCartao _transacaoCartao;

        public TransacaoCartao TransacaoCartao => _transacaoCartao;

        public ProcessandoTransacaoCartaoEvent(TransacaoCartao transacaoCartao) 
            => _transacaoCartao = transacaoCartao;
    }
}