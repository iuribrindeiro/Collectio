using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class FalhaAoProcessarFormaPagamentoEvent : IDomainEvent
    {
        private Cobranca _cobranca;
        public Cobranca Cobranca => _cobranca;

        public FalhaAoProcessarFormaPagamentoEvent(Cobranca cobranca)
            => _cobranca = cobranca;
    }
}