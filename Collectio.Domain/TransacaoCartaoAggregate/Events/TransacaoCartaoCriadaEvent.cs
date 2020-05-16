using Collectio.Domain.Base;

namespace Collectio.Domain.TransacaoCartaoAggregate.Events
{
    public class TransacaoCartaoCriadaEvent : IDomainEvent
    {
        private TransacaoCartao _transacaoCartao;
        public TransacaoCartao TransacaoCartao => _transacaoCartao;

        public TransacaoCartaoCriadaEvent(TransacaoCartao transacaoCartao) 
            => _transacaoCartao = transacaoCartao;
    }
}