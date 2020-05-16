using Collectio.Domain.Base;

namespace Collectio.Domain.TransacaoCartaoAggregate.Events
{
    public class TransacaoCartaoAprovadaEvent : IDomainEvent
    {
        private TransacaoCartao _transacaoCartao;

        public TransacaoCartao TransacaoCartao => _transacaoCartao;

        public TransacaoCartaoAprovadaEvent(TransacaoCartao transacaoCartao) 
            => _transacaoCartao = transacaoCartao;
    }
}