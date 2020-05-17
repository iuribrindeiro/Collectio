using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class CobrancaCriadaEvent : IDomainEvent
    {
        private string _cobrancaId;

        public string CobrancaId => _cobrancaId;

        public CobrancaCriadaEvent(string cobrancaId) 
            => _cobrancaId = cobrancaId;

    }
}