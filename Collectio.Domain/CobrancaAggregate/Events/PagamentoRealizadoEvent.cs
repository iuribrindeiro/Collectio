using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class PagamentoRealizadoEvent : IDomainEvent
    {
        private Cobranca _cobranca;
        public Cobranca Cobranca => _cobranca;

        public PagamentoRealizadoEvent(Cobranca cobranca) 
            => _cobranca = cobranca;
    }
}