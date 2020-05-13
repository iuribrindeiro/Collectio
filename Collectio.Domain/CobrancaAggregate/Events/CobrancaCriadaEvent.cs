using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class CobrancaCriadaEvent : IDomainEvent
    {
        private Cobranca _cobranca;

        public Cobranca Cobranca => _cobranca;

        public CobrancaCriadaEvent(Cobranca cobranca) 
            => _cobranca = cobranca;

    }
}