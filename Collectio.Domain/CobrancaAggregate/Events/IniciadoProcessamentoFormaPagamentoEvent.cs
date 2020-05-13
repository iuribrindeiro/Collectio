using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class IniciadoProcessamentoFormaPagamentoEvent : IDomainEvent
    {
        private Cobranca _cobranca;
        public Cobranca Cobranca => _cobranca;

        public IniciadoProcessamentoFormaPagamentoEvent(Cobranca cobranca) 
            => _cobranca = cobranca;
    }
}