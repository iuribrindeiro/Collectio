using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class FormaPagamentoProcessadaEvent : IDomainEvent
    {
        private Cobranca _cobranca;
        public Cobranca Cobranca => _cobranca;

        public FormaPagamentoProcessadaEvent(Cobranca cobranca)
            => _cobranca = cobranca;
    }
}