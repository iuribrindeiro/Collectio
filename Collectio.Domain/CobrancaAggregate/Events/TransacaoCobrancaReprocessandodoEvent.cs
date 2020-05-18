using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate.Events
{
    public class TransacaoCobrancaReprocessandodoEvent : IDomainEvent
    {
        private string _cobrancaId;

        public string CobrancaId => _cobrancaId;

        public TransacaoCobrancaReprocessandodoEvent(string cobrancaId) 
            => _cobrancaId = cobrancaId;
    }
}